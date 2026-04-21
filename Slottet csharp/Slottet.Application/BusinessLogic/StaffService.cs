using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class StaffService
{
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _unitOfWork;

    public StaffService(IStaffRepository staffRepo, IUnitOfWork unitOfWork)
    {
        _staffRepo = staffRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<StaffDto> Add(StaffDto staffDto)
    {
            Staff staff = Staff.Create(
                 staffDto.Initials ?? string.Empty,
                 staffDto.FirstName ?? string.Empty,
                 staffDto.LastName ?? string.Empty,
                 staffDto.Email ?? string.Empty,
                 staffDto.DepartmentId,
                 staffDto.RoleId
            );
              
        await _staffRepo.Add(staff);        

        // Checks if the staff was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede medarbejder i databasen.");

        // Fetches the staff to get the generated ID by the DB and related entities
        Staff? created = await _staffRepo.GetById(staff.Id);
        if (created == null)
            throw new InvalidOperationException("Staff kunne ikke findes efter oprettelsen");

        // Maps the created staff to a DTO and returns it API
        return MapToDto(created);
    }

    public async Task<StaffDto> GetById(int id)
    {
        Staff? staff = await _staffRepo.GetById(id);

        if (staff == null)
            throw new InvalidOperationException($"Staff med {id} blev ikke fundet");
        
        // Maps the staff to a DTO and returns it API
        return MapToDto(staff);
    }

    public async Task<List<StaffDto>> GetAll()
    {
        List<Staff> staffList = await _staffRepo.GetAll();

        if (staffList == null || staffList.Count == 0)
            throw new InvalidOperationException("Ingen medarbejdere blev fundet");

        // Takes each staff in the list and sends it to the MapToDto method, and returns it
        return staffList.Select(MapToDto).ToList();
    }

    public async Task Update(StaffDto staffDto)
    {
        Staff? staffToUpdate = await _staffRepo.GetById(staffDto.Id);
        if (staffToUpdate == null)
            throw new InvalidOperationException($"Staff med ID {staffDto.Id} blev ikke fundet");

        // Updates the old staff object with the new values with the Domain-layer's validation
        staffToUpdate.UpdateStaffDetails(
            staffDto.Initials ?? string.Empty,
            staffDto.FirstName ?? string.Empty,
            staffDto.LastName ?? string.Empty,
            staffDto.Email ?? string.Empty,
            staffDto.DepartmentId,
            staffDto.RoleId
            );        

        _staffRepo.Update(staffToUpdate);
        
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke opdatere medarbejderen i databasen.");

    }

    public async Task Delete(int id)
    {
        Staff? staffToDelete = await _staffRepo.GetById(id);
        if (staffToDelete == null)
            throw new InvalidOperationException($"Staff med ID {id} blev ikke fundet og kunne ikke slettes");
            
        _staffRepo.Delete(staffToDelete);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette medarbejderen i databasen.");
    }

    // Helper method to prevent code duplication
    private StaffDto MapToDto(Staff staff)
    {
        return new StaffDto
        {
            Id = staff.Id,
            Initials = staff.Initials ?? string.Empty,
            FirstName = staff.FirstName,
            LastName = staff.LastName,
            Email = staff.Email,
            DepartmentId = staff.DepartmentId,
            RoleId = staff.RoleId,

            Department = staff.Department?.DepartmentName ?? "",
            RoleName = staff.Role?.RoleName ?? ""
        };
    }
}
