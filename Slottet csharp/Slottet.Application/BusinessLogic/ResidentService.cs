using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

// Orchestrates all business logic related to Residents.
// Acts as the middleman between the API controller and the database residentRepo.

public class ResidentService
{
    private readonly IResidentRepository _residentRepo;

    // The residentRepo interface is injected via Dependency Injection
    public ResidentService(IResidentRepository residentRepo)
    {
        _residentRepo = residentRepo;
    }

    // Validates data, maps the DTO to a Domain entity, and saves it.
    // Returns True if successful, False if validation failed.
    public async Task<bool> ExecuteAsync(ResidentDto dto)
    {
        // 1. Business Validation: Ensure initials are not empty or just whitespaces
        if (string.IsNullOrWhiteSpace(dto.Initial))
        {
            return false;
        }

        // 2. Mapping: Convert the incoming DTO into a valid Domain entity
        var resident = Resident.Create(dto.Initial);

        // 3. Persistence: Add the entity to memory and save changes to the SQL database
        await _residentRepo.AddAsync(resident);
        var rowsAffected = await _residentRepo.SaveChangesAsync();

        // 4. Verification: Return true if at least one row was added to the database
        return rowsAffected > 0;
    }
}