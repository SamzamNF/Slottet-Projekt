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

    // The residentRepo interface is injected via Dependency Injection
    public ResidentService(IResidentRepository residentRepo, IUnitOfWork unitOfWork)
    {
        _residentRepo = residentRepo;
        _unitOfWork = unitOfWork;
    }

    // Validates data, maps the DTO to a Domain entity, and saves it.
    // Returns ResidentDto
    public async Task<ResidentDto> ExecuteAsync(ResidentDto dto)
    {
        // Call factory method on Resident entity to create a new instance based on the data in the DTO.
        var resident = Resident.Create(dto.Initial);

        // Await the repository method to add the new resident to the database context.
        await _residentRepo.AddAsync(resident);

        // Call SaveChangesAsync on the UnitOfWork to persist the new resident to the database.
        await _unitOfWork.SaveChangesAsync();

        // Update DTO with Id returned from database
        dto.Id = resident.Id;

        // Return DTO, now with updated Id
        return dto;
    }
}