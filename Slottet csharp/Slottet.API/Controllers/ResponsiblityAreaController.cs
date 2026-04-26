using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;

namespace Slottet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsiblityAreaController : ControllerBase
    {
        private readonly ResponsibilityAreaService _responsibilityAreaService;

        public ResponsiblityAreaController(ResponsibilityAreaService responsibilityAreaService)
        {
            _responsibilityAreaService = responsibilityAreaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(ResponsibilityAreaDTO responsibilityAreaDto)
        {
            try
            {
                ResponsibilityAreaDTO addedResponsibilityArea = await _responsibilityAreaService.Add(responsibilityAreaDto);
                return Ok(addedResponsibilityArea);
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Du skal angive et gyldigt ID større end 0.");

                ResponsibilityAreaDTO responsibilityArea = await _responsibilityAreaService.GetById(id);
                return Ok(responsibilityArea);
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
                List<ResponsibilityAreaDTO> responsibilityAreas = await _responsibilityAreaService.GetAll();
                return Ok(responsibilityAreas);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(ResponsibilityAreaDTO responsibilityAreaDto)
        {
            try
            {
                await _responsibilityAreaService.Update(responsibilityAreaDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"Fejl ved opdatering: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved opdatering: {ex.Message}");
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

                await _responsibilityAreaService.Delete(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Fejl ved sletning: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Fejl ved sletning: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Uventet fejl: {ex.Message}");
            }
        }
    }
}
