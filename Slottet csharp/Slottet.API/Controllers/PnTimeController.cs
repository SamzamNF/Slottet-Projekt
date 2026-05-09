using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;


namespace Slottet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PnTimeController : ControllerBase
    {
        private readonly PnTimeService _pnTimeService;

        public PnTimeController(PnTimeService pnTimeService)
        {
            _pnTimeService = pnTimeService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Personale")]
        public async Task<IActionResult> Add(PnTimeDTO pnTimeDto)
        {
            try
            {
                PnTimeDTO addedPnTime = await _pnTimeService.Add(pnTimeDto);
                return Ok(addedPnTime);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"Fejl ved oprettelse: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved oprettelse: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Uventet fejl: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Personale")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                PnTimeDTO pnTime = await _pnTimeService.GetById(id);
                return Ok(pnTime);
            }
            catch (KeyNotFoundException ex)
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
                List<PnTimeDTO> pnTimes = await _pnTimeService.GetAllPnTimes();
                return Ok(pnTimes);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved hentning: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Uventet fejl: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Personale")]
        public async Task<IActionResult> Update(PnTimeDTO pnTimeDto)
        {
            try
            {
                await _pnTimeService.Update(pnTimeDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved opdatering: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"Fejl ved opdatering: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Uventet fejl: {ex.Message}");
            }
        }

        // Only Admins can delete PnTimes, if needed can always be changed to allow permission for "Personale" role as well
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Ugyldigt ID: ID skal være større end 0.");

                await _pnTimeService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved sletning: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"Fejl ved sletning: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Uventet fejl: {ex.Message}");
            }
        }
    }
}
