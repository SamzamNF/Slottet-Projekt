using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;

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
    public async Task<IActionResult> Add(ResidentDto dto)
    {
        try
        {
            // Call service. If it fails: line is interrupted, code jumps directly to 'catch'.
            var addedResident = await _residentService.ExecuteAsync(dto);

            // Returns 201 Created with newly created Resident object (now including ID)
            return Created(string.Empty, addedResident);
        }
        catch (ArgumentException ex)
        {
            // Catches user errors (fx domain validations)
            return BadRequest($"Ugyldigt input: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Catches all other unexpected errors
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }

    // Edpoint to update an existing resident
    // URL: PUT api/resident/{id}
    [HttpPut("{id}")]
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
    public async Task<IActionResult> Archive(int id)
    {
        try
        {
            await _residentService.ArchiveAsync(id);
            return NoContent();
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
}