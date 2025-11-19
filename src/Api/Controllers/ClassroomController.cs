using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTOs.ClassroomDTO;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomServices _service;

    public ClassroomController(IClassroomServices service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClassrooms()
    {
        var classrooms = await _service.GetAllAsync();
        return Ok(classrooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassroomById(int id)
    {
        var classroom = await _service.GetByIdAsync(id);

        if (classroom == null)
            return NotFound(new { error = "Classroom not found" });

        return Ok(classroom);
    }

    [HttpPost]
    public async Task<IActionResult> AddClassroom([FromBody] ClassroomCreateDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { message = "Classroom added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClassroom(int id, [FromBody] ClassroomUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { message = "Classroom updated successfully!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassroom(int id)
    {
        await _service.RemoveAsync(id);
        return Ok(new { message = "Classroom deleted successfully!" });
    }
}