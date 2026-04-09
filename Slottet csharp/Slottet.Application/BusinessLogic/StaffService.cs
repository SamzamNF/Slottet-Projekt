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
              
        var addedStaff = await _staffRepo.Add(staff);
        
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede medarbejder i databasen.");

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
