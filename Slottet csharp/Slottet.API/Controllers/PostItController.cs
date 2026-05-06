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

    [HttpGet("history")]
    [Authorize(Roles = "Admin,Personale")]
    // [FromQuery] tells the controller to look for the 'date' parameter in the URL query string
    public async Task<IActionResult> GetHistory([FromQuery] DateTime date)
    {
        try
        {
            var history = await _postItService.GetHistory(date);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest($"Uventet fejl: {ex.Message}");
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
        catch (Exception ex)
        {
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> CreatePostIt([FromBody] PostItDTO postItDTO)
    {
        try
        {
            var created = await _postItService.Create(postItDTO);
            return Ok(created);
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

    [HttpPut]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> UpdatePostIt([FromBody] PostItDTO postItDTO)
    {
        try
        {
            var updatedPostIt = await _postItService.Update(postItDTO);
            return Ok(updatedPostIt);
        }
        
        catch (Exception ex)
        {
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Personale")]
    public async Task<IActionResult> DeletePostIt(int id)
    {
        try
        {
            await _postItService.Delete(new PostItDTO { Id = id });
            return Ok("PostIt slettet");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }
}