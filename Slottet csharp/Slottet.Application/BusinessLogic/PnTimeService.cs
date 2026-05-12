using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class PnTimeService
{
    private readonly IPnTimeRepository _pnTimeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PnTimeService(IPnTimeRepository pnTimeRepository, IUnitOfWork unitOfWork)
    {
        _pnTimeRepository = pnTimeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PnTimeDTO>> GetAllPnTimes()
    {
        List<PnTime> pnTimes = await _pnTimeRepository.GetAll();
        if (pnTimes == null || pnTimes.Count == 0)
        {
            throw new KeyNotFoundException("Ingen Pn Tider fundet");
        }

        return pnTimes.Select(p => MapToDTO(p)).ToList();
    }

    public async Task<PnTimeDTO> GetById(int id)
    {
        PnTime? pnTime = await _pnTimeRepository.GetById(id);
        if (pnTime == null)
            throw new KeyNotFoundException($"Pn Tid med ID: {id} blev ikke fundet");

        return MapToDTO(pnTime);
    }

    public async Task<PnTimeDTO> Add(PnTimeDTO pnTimeDto)
    {
        PnTime pnTime = PnTime.Create(pnTimeDto.Time, pnTimeDto.Description);

        await _pnTimeRepository.Add(pnTime);

        // Checks if the PnTime was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede Pn Tid i databasen.");
        
        PnTime? created = await _pnTimeRepository.GetById(pnTime.Id);
        if (created == null)
            throw new InvalidOperationException("Kunne ikke finde den oprettede Pn Tid i databasen.");

        return MapToDTO(created);
    }

    public async Task Update(PnTimeDTO pnTimeDto)
    {
        PnTime? pnTime = await _pnTimeRepository.GetById(pnTimeDto.Id);
        if (pnTime == null)
            throw new KeyNotFoundException($"PnTime med ID: {pnTimeDto.Id} blev ikke fundet");

        // Updates the PnTime with the new values from the DTO
        pnTime.Update(pnTimeDto.Time, pnTimeDto.Description);

        // Saves the updated PnTime to the database and checks if the update was successful, throwing an exception if not
        _pnTimeRepository.Update(pnTime);
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den opdaterede Pn Tid i databasen.");
    }

    public async Task Delete(int id)
    {
        PnTime? pnTime = await _pnTimeRepository.GetById(id);
        if (pnTime == null)
            throw new KeyNotFoundException($"PnTime med ID: {id} blev ikke fundet");

        _pnTimeRepository.Delete(pnTime);

        // Checks if the PnTime was deleted from the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette Pn Tid fra databasen.");
    }

    private PnTimeDTO MapToDTO(PnTime pnTime)
    {
        return new PnTimeDTO
        {
            Id = pnTime.Id,
            Time = pnTime.Time,
            Description = pnTime.Description
        };
    }

}
