using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [HttpGet("resident/{residentId}")]
    public async Task<IActionResult> GetByResident(int residentId)
    {
        var result = await _medicineService.GetByResidentAsync(residentId);
        return Ok(result);
    }

    [HttpPost]
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

    // Pass new description as a simple string value in body
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] string newDescription)
    {
        try
        {
            await _medicineService.UpdateAsync(id, newDescription);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Catch IsFromToday exception
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
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