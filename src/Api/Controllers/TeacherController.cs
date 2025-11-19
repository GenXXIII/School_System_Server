using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherServices _service;

        public TeacherController(ITeacherServices service)
        {
            _service = service;
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<IActionResult> GetAllTeacher()
        {
            try
            {
                var teachers = await _service.GetAllAsync();
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving teachers: {ex.Message}" });
            }
        }

        // GET: api/Teacher/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            try
            {
                var teacher = await _service.GetByIdAsync(id);
                if (teacher == null)
                    return NotFound(new { error = "Teacher not found" });

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving teacher: {ex.Message}" });
            }
        }
        // POST: api/Teacher
        [HttpPost]
        public async Task<IActionResult> AddTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest(new { error = "Teacher data is required" });
            try
            {
                await _service.AddAsync(teacher);

                // Return the newly created teacher as JSON
                return CreatedAtAction(
                    nameof(GetTeacherById),
                    new { id = teacher.Id },
                    new { message = "Teacher added successfully!", teacher }
                );
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { error = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Teacher/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest(new { error = "Teacher data is required" });

            if (teacher.Id != id)
                teacher.Id = id;

            try
            {
                await _service.UpdateAsync(teacher);

                return Ok(new { message = "Student updated successfully!", teacher });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/Teacher/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                await _service.RemoveAsync(id);
                return Ok(new { message = "Teacher deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
