using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Slottet.API.Controllers;

// Handles incoming HTTP requests related to Residents.

[Route("api/[controller]")]
[ApiController]
public class ResidentController : ControllerBase
{
    private readonly ResidentService _residentService;

    public ResidentController(ResidentService residentService)
    {
        _residentService = residentService;
    }

    // Endpoint to create a new resident.
    // URL: POST /api/resident
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(ResidentDto dto)
    {
        try
        {
            // Call service. If it fails: line is interrupted, code jumps directly to 'catch'.
            var addedResident = await _residentService.ExecuteAsync(dto);

            // Returns 201 Created with newly created Resident object (now including ID)
            return Ok(addedResident);
        }
        catch (ArgumentException ex)
        {
            // Catches user errors (fx domain validations)
            return BadRequest($"Ugyldigt input: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            // Catches errors related to database operations
            return StatusCode(500, $"Fejl ved oprettelse: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Catches all other unexpected errors
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Personale")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Du skal angive et gyldigt ID større end 0.");

            var resident = await _residentService.GetByIdAsync(id);
            return Ok(resident);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Fejl: {ex.Message}");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Personale")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<ResidentDto> residents = await _residentService.GetAllAsync();
            return Ok(residents);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved henting af liste: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }

    // Endpoint to update an existing resident
    // URL: PUT api/resident/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ResidentDto dto)
    {
        try
        {
            var updated = await _residentService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Fejl: {ex.Message}");
        }
    }

    // Endpoint to archive (soft delete) a resident
    // URL: DELETE api/resident/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Archive(int id)
    {
        try
        {
            await _residentService.ArchiveAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Beboer ikke fundet: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Fejl: {ex.Message}");
        }
    }

    [HttpDelete("{id}/permanent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _residentService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Beboer ikke fundet: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, $"Fejl ved sletning: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Fejl: {ex.Message}");
        }
    }


}