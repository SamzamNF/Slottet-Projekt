using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slottet.Application.BusinessLogic;
using Slottet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Slottet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;
        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DepartmentDTO departmentDto)
        {
            try
            {
                DepartmentDTO addedDepartment = await _departmentService.Add(departmentDto);
                return Ok(addedDepartment);
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

                DepartmentDTO department = await _departmentService.GetById(id);
                return Ok(department);
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
                List<DepartmentDTO> departments = await _departmentService.GetAllDepartments();
                return Ok(departments);
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
        public async Task<IActionResult> Update(DepartmentDTO departmentDto)
        {
            try
            {
                await _departmentService.Update(departmentDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldigt input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Fejl ved opdatering: {ex.Message}");
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

                await _departmentService.Delete(id);
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
             catch (Exception ex)             {
                 return StatusCode(500, $"Uventet fejl: {ex.Message}");
             }
        }
    }
}
