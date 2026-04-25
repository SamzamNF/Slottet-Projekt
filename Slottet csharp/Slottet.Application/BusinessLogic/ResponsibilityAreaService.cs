using System;
using Slottet.Domain.Entities;
using Slottet.Application.Interfaces;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class ResponsibilityAreaService
{
    private readonly IResponsibilityAreaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ResponsibilityAreaService(IResponsibilityAreaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ResponsibilityAreaDTO>> GetAll()
    {
        List<ResponsibilityArea> responsibilityAreas = await _repository.GetAll();
        if (responsibilityAreas == null || responsibilityAreas.Count == 0)
        {
            throw new KeyNotFoundException("Ingen ansvarsområder fundet");
        }

        // Takes each responsibility area in the list and transforms it to a DTO and returns it to the API
        return responsibilityAreas.Select(a => MapToDto(a)).ToList();
    }

    public async Task<ResponsibilityAreaDTO> GetById(int id)
    {
        ResponsibilityArea? responsibilityArea = await _repository.GetById(id);
        
        if (responsibilityArea == null)
            throw new KeyNotFoundException($"Ansvarsområde med ID {id} blev ikke fundet");

        // Maps the responsibility area to a DTO and returns it to the API
        return MapToDto(responsibilityArea);
    }

    public async Task<ResponsibilityAreaDTO> Add(ResponsibilityAreaDTO responsibilityAreaDto)
    {
        ResponsibilityArea responsibilityArea = ResponsibilityArea.Create(
            responsibilityAreaDto.Description ?? string.Empty,
            responsibilityAreaDto.Date,
            responsibilityAreaDto.StaffId,
            responsibilityAreaDto.PhoneId
        );

        await _repository.Add(responsibilityArea);

        // Checks if the responsibility area was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme det oprettede ansvarsområde i databasen.");

        // Gets the responsibility area to get the generated ID from the DB, and returns it to the API as a DTO
        ResponsibilityArea? created = await _repository.GetById(responsibilityArea.Id);
        if (created == null)
            throw new KeyNotFoundException("Ansvarsområde kunne ikke findes efter oprettelsen");

        // Maps the created responsibility area to a DTO and returns it API
        return MapToDto(created);  
    }

    public async Task Update(ResponsibilityAreaDTO responsibilityAreaDTO)
    {
        ResponsibilityArea? responsibilityArea = await _repository.GetById(responsibilityAreaDTO.Id);
        if (responsibilityArea == null)
            throw new KeyNotFoundException($"Ansvarsområde med ID {responsibilityAreaDTO.Id} blev ikke fundet");
        
        // Updates the responsibility area with the new values from the DTO
        responsibilityArea.Update(
            responsibilityAreaDTO.Description ?? string.Empty,
            responsibilityAreaDTO.Date,
            responsibilityAreaDTO.StaffId,
            responsibilityAreaDTO.PhoneId
        );

        _repository.Update(responsibilityArea);

        // Checks if the responsibility area was updated in the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke opdatere ansvarsområdet i databasen.");
    }

    public async Task Delete(int id)
    {
        ResponsibilityArea? responsibilityArea = await _repository.GetById(id);
        if (responsibilityArea == null)
            throw new KeyNotFoundException($"Ansvarsområde med ID {id} blev ikke fundet");

        _repository.Delete(responsibilityArea);

        // Checks if the responsibility area was deleted from the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette ansvarsområdet fra databasen.");
    }

    private ResponsibilityAreaDTO MapToDto(ResponsibilityArea rArea)
    {
        if (rArea.Phone == null)
            throw new ArgumentNullException("Phone data mangler for ansvarsområde med ID");;
        if (rArea.Staff == null)
            throw new ArgumentNullException("Staff data mangler for ansvarsområde med ID");

        return new ResponsibilityAreaDTO
        {
            Id = rArea.Id,
            Description = rArea.Description,
            Date = rArea.Date,
            StaffId = rArea.StaffId,
            PhoneId = rArea.PhoneId,
            StaffInitials = rArea.Staff.Initials,
            PhoneNumber = rArea.Phone.PhoneNumber
        };
    }


}
