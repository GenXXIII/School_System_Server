using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.TeacherDTO;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherServices _service;

    public TeacherController(ITeacherServices service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _service.GetAllAsync();
        return Ok(teachers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        var teacher = await _service.GetByIdAsync(id);

        if (teacher == null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> AddTeacher([FromBody] TeacherCreateDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { message = "Teacher added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { message = "Teacher updated successfully!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Teacher deleted successfully!" });
    }
}