using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class StaffService
{
    private readonly IStaffRepository _staffRepo;

    public StaffService(IStaffRepository staffRepo)
    {
        _staffRepo = staffRepo;
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
              
        var addedStaff = await _staffRepo.Add(staff);

        // missing a GetRole and GetDepartment to match rolename/departmentname in StaffDto

        return new StaffDto
        {
            
            Id = addedStaff.Id,
            Initials = addedStaff.Initials ?? string.Empty,
            FirstName = addedStaff.FirstName,
            LastName = addedStaff.LastName,
            Email = addedStaff.Email,
            DepartmentId = addedStaff.DepartmentId,
            RoleId = addedStaff.RoleId,

            Department = "DepartmentNamePlaceholder",
            RoleName = "RoleNamePlaceholder"
        };
    }

}
