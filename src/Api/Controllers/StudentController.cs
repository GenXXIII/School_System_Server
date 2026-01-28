using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.StudentDTO;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentController : ControllerBase
{
    private readonly IStudentServices _service;

    public StudentController(IStudentServices service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _service.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _service.GetByIdAsync(id);

        if (student == null)
            return NotFound(new { error = "Student not found" });

        return Ok(student);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> AddStudent([FromBody] StudentCreateDto dto)
    {
        try
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Student added successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Student updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Student deleted successfully!" });
    }
}