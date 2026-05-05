using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Slottet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly PhoneService _phoneService;

        public PhoneController(PhoneService phoneService)
        {
            _phoneService = phoneService;
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Add(PhoneDTO phoneDTO)
        {
            try
            {
                PhoneDTO addedPhone = await _phoneService.Add(phoneDTO);
                return Ok(addedPhone);
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
                PhoneDTO phone = await _phoneService.GetById(id);
                return Ok(phone);
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
                List<PhoneDTO> phones = await _phoneService.GetAllPhones();
                return Ok(phones);
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
        public async Task<IActionResult> Update(PhoneDTO phoneDTO)
        {
            try
            {
                await _phoneService.Update(phoneDTO);
                return NoContent();
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
                    return BadRequest("Ugyldigt ID: ID skal være større end 0.");
                    
                await _phoneService.Delete(id);
                return NoContent();
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
}
