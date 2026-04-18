using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;


namespace Slottet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Add(RoleDTO roleDto)
        {
            try
            {
                RoleDTO addedRole = await _roleService.Add(roleDto);
                return Ok(addedRole);
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

                RoleDTO role = await _roleService.GetById(id);
                return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Fejl ved hentning: {ex.Message}");
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
                List<RoleDTO> roles = await _roleService.GetAllRoles();
                return Ok(roles);
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
        public async Task<IActionResult> Update(RoleDTO roleDto)
        {
            try
            {
                await _roleService.Update(roleDto);
                return NoContent();
            }
             catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
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
                await _roleService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
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
