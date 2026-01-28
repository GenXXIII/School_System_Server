using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.DepartmentDTO;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentServices _service;

    public DepartmentController(IDepartmentServices service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _service.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await _service.GetByIdAsync(id);

        if (department == null)
            return NotFound(new { error = "Department not found" });

        return Ok(department);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDepartment([FromBody] DepartmentCreateDto dto)
    {
        try
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Department added successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Department updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Department deleted successfully!" });
    }
}
