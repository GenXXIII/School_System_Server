using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.CourseDTO;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseServices _service;

    public CourseController(ICourseServices service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _service.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var course = await _service.GetByIdAsync(id);

        if (course == null)
            return NotFound(new { error = "Course not found" });

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> AddCourse([FromBody] CourseCreateDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { message = "Course added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { message = "Course updated successfully!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Course deleted successfully!" });
    }
}