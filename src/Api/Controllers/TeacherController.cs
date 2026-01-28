using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.TeacherDTO;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TeacherController : ControllerBase
{
    private readonly ITeacherServices _service;

    public TeacherController(ITeacherServices service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _service.GetAllAsync();
        return Ok(teachers);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        var teacher = await _service.GetByIdAsync(id);

        if (teacher == null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(teacher);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTeacher([FromBody] TeacherCreateDto dto)
    {
        try
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Teacher added successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Teacher updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Teacher deleted successfully!" });
    }
}