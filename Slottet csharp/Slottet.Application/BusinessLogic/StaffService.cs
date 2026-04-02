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

    public async Task<StaffDto> Add(Staff staff)
    {
        var addedStaff = await _staffRepo.Add(staff);

        // missing a GetRole and GetDepartment to match rolename/departmentname in StaffDto

        return new StaffDto
        {
            Initials = addedStaff.Initials,
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
