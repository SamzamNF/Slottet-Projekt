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
            var staff = Staff.Create(
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

    // Helper method to prevent code duplication when fetching staff by ID
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
