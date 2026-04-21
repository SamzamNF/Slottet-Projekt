using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
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
            StaffDto addedStaff = await _staffService.Add(staffDto);
            return Ok(addedStaff);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Ugyldigt input: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, $"Fejl ved oprettelse: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Du skal angive et gyldigt ID større end 0.");

            StaffDto staff = await _staffService.GetById(id);
            return Ok(staff);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved hentning: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<StaffDto> staffList = await _staffService.GetAll();
            return Ok(staffList);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved hentning: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(StaffDto staffDto)
    {
        try
        {
            if (staffDto.Id <= 0)
                return BadRequest("Du skal angive et gyldigt ID større end 0.");

            await _staffService.Update(staffDto);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved opdatering: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Ugyldigt input: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Du skal angive et gyldigt ID større end 0.");

            await _staffService.Delete(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved sletning: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }
}