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

    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var list = await _postItService.GetAllPostIts();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest($"Uventet fejl: {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> UpdatePostIt(PostItDTO postItDTO)
    {
        try
        {
            var updatedPostIt = await _postItService.UpdateInfo(postItDTO);
            return Ok(updatedPostIt);
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
}