using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

// Orchestrates all business logic related to Residents.
// Acts as the middleman between the API controller and the database residentRepo.

public class ResidentService
{
    private readonly IResidentRepository _residentRepo;
    private readonly IUnitOfWork _unitOfWork;

    // residentRepo interface, injected via Dependency Injection
    public ResidentService(IResidentRepository residentRepo, IUnitOfWork unitOfWork)
    {
        _residentRepo = residentRepo;
        _unitOfWork = unitOfWork;
    }

    // Validate data, map DTO to a Domain entity, save it
    // Return ResidentDto
    public async Task<ResidentDto> ExecuteAsync(ResidentDto dto)
    {
        // Call factory method on Resident entity to create a new instance based on the data in the DTO
        var resident = Resident.Create(dto.Initial);

        // Await repository method to add new resident to database context
        await _residentRepo.AddAsync(resident);

        // Call SaveChangesAsync on UnitOfWork to persist new resident to database
        await _unitOfWork.SaveChangesAsync();

        // Update DTO with Id returned from database
        dto.Id = resident.Id;

        // Return DTO, now with updated Id
        return dto;
    }

    // Update resident information
    public async Task<ResidentDto> UpdateAsync(int id, ResidentDto dto)
    {
        var resident = await _residentRepo.GetByIdAsync(id);
        if (resident == null)
            throw new KeyNotFoundException("Beboeren blev ikke fundet.");

        resident.Update(dto.Initial);
        await _unitOfWork.SaveChangesAsync();

        dto.Id = resident.Id;
        dto.IsArchived = resident.IsArchived;
        return dto;
    }

    // Delete (Archive) resident by ID
    public async Task ArchiveAsync(int id)
    {
        var resident = await _residentRepo.GetByIdAsync(id);
        if (resident == null)
            throw new KeyNotFoundException("Beboeren blev ikke fundet.");

        resident.Archive();
        await _unitOfWork.SaveChangesAsync();
    }
}