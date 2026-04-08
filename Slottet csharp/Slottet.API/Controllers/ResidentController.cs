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
        // Pass the request to the Application layer for processing
        var isSuccess = await _residentService.ExecuteAsync(dto);

        // If validation failed in the service layer, return HTTP 400 Bad Request
        if (!isSuccess)
        {
            return BadRequest("Fejl - initialer mangler");
        }

        // If successful, return HTTP 201 Created
        return Created(string.Empty, dto);
    }
}