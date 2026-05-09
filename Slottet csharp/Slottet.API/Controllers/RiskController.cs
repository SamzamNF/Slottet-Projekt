using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;

namespace Slottet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiskController : ControllerBase
{
    private readonly RiskService _riskService;

    public RiskController(RiskService riskService)
    {
        _riskService = riskService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RiskDTO>> GetRisk(int id)
    {
        var risk = await _riskService.GetRiskAsync(id);

        if (risk == null)
            return NotFound();

        return Ok(risk);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRisk(int id, [FromBody] RiskDTO riskDto)
    {
        if (id != riskDto.Id)
            return BadRequest();

        var updated = await _riskService.UpdateRiskAsync(riskDto);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}