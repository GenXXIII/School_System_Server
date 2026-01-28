using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.CourseDTO;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseServices _service;

    public CourseController(ICourseServices service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _service.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var course = await _service.GetByIdAsync(id);

        if (course == null)
            return NotFound(new { error = "Course not found" });

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> AddCourse([FromBody] CourseCreateDto dto)
    {
        try
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Course added successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Course updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Course deleted successfully!" });
    }
}