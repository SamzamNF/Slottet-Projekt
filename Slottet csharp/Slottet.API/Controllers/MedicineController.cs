using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;

namespace Slottet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
    private readonly MedicineService _medicineService;

    public MedicineController(MedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    // Accessible by Admin + Personale
    [HttpGet("resident/{residentId}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> GetByResident(int residentId)
    {
        var result = await _medicineService.GetByResidentAsync(residentId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> Create([FromBody] MedicineDto dto)
    {
        try
        {
            var result = await _medicineService.CreateAsync(dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> Update(int id, [FromBody] string newDescription)
    {
        try
        {
            // Extract the role from user JWT token
            bool isAdmin = User.IsInRole("Admin");

            await _medicineService.UpdateAsync(id, newDescription);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool isAdmin = User.IsInRole("Admin");

            await _medicineService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}