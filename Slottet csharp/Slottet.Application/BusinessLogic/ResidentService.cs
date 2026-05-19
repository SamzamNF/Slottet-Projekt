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
        var resident = Resident.Create(dto.Initial, dto.DepartmentId);

        // Await repository method to add new resident to database context
        await _residentRepo.AddAsync(resident);

        // Call SaveChangesAsync on UnitOfWork to persist new resident to database
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede beboer i databasen.");

        Resident? created = await _residentRepo.GetByIdAsync(resident.Id);
        if (created == null)
            throw new InvalidOperationException("Beboeren kunne ikke findes efter oprettelsen");

        // Return DTO, now with updated Id
        return MapToDto(created);
    }

    public async Task<ResidentDto> GetByIdAsync(int id)
    {
        Resident? resident = await _residentRepo.GetByIdAsync(id);
        if (resident == null)
            throw new InvalidOperationException($"Beboer med {id} blev ikke fundet");
        
        return MapToDto(resident);
    }

    public async Task<List<ResidentDto>> GetAllAsync()
    {
        List<Resident> residents = await _residentRepo.GetAllAsync();
        if (residents == null || residents.Count == 0)
            throw new InvalidOperationException("Ingen beboere blev fundet");

        return residents.Select(MapToDto).ToList();
    }

    // Update resident information
    public async Task<ResidentDto> UpdateAsync(int id, ResidentDto dto)
    {
        var resident = await _residentRepo.GetByIdAsync(id);
        if (resident == null)
            throw new KeyNotFoundException("Beboeren blev ikke fundet.");

        resident.Update(dto.Initial, dto.DepartmentId);

        _residentRepo.Update(resident);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den opdaterede beboer i databasen.");

        return MapToDto(resident);
    }

    public async Task DeleteAsync(int id)
    {
        Resident? resident = await _residentRepo.GetByIdAsync(id);
        if (resident == null)
            throw new KeyNotFoundException("Beboeren blev ikke fundet.");

        _residentRepo.Delete(resident);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette beboeren i databasen.");
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

    private ResidentDto MapToDto(Resident resident)
    {
        return new ResidentDto
        {
            Id = resident.Id,
            Initial = resident.Initial ?? string.Empty,
            DepartmentId = resident.DepartmentId,
            DepartmentName = resident.Department?.DepartmentName ?? string.Empty,
            IsArchived = resident.IsArchived
        };
    }
}