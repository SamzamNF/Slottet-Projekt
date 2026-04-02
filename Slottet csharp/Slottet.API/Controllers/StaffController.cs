using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly StaffService _staffService;

    public StaffController(StaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(StaffDto staffDto)
    {
        try
        {
            var staff = new Staff
            {
                Initials = staffDto.Initials,
                FirstName = staffDto.FirstName,
                LastName = staffDto.LastName,
                Email = staffDto.Email,
                DepartmentId = staffDto.DepartmentId,
                RoleId = staffDto.RoleId
            };

            var addedStaff = await _staffService.Add(staff);
            return Ok(addedStaff);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Ugyldigt input: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Uvenetet fejl: {ex.Message}");
        }
    }
}