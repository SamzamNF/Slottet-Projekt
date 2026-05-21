using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;

namespace Slottet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostItController : ControllerBase
{
    private readonly PostItService _postItService;

    public PostItController(PostItService postItService)
    {
        _postItService = postItService;
    }

    [HttpGet("history/date")]
    [Authorize(Roles = "Admin,Personale")]
    // [FromQuery] tells the controller to look for the 'date' parameter in the URL query string
    public async Task<IActionResult> GetHistory([FromQuery] DateTime date)
    {
        try
        {
            var history = await _postItService.GetHistory(date);
            return Ok(history);
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

    [HttpGet("all-history")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> GetAllHistory()
    {
        try
        {
            var history = await _postItService.GetAllHistory();
            return Ok(history);
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

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Du skal angive et gyldigt ID større end 0.");
            
            var postIt = await _postItService.GetById(id);
            return Ok(postIt);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound($"Fejl ved hentning: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Fejl ved hentning fra database: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Uventet fejl: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> Create([FromBody] PostItDTO postItDTO)
    {
        try
        {
            var created = await _postItService.Create(postItDTO);
            return Ok(created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($" Ugyldig input: {ex.Message}");
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

    [HttpPut]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> Update([FromBody] PostItDTO postItDTO)
    {
        try
        {
            var updatedPostIt = await _postItService.Update(postItDTO);
            return Ok(updatedPostIt);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Fejl ved opdatering: {ex.Message}");
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
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _postItService.Delete(id);
            return Ok("Post-It slettet");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Fejl ved sletning: {ex.Message}");
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