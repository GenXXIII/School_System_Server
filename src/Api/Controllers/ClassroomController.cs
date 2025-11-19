using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomServices _service;

        public ClassroomController(IClassroomServices service)
        {
            _service = service;
        }

        // GET: api/Classroom
        [HttpGet]
        public async Task<IActionResult> GetAllClass()
        {
            try
            {
                var classrooms = await _service.GetAllAsync();
                return Ok(classrooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving classroom: {ex.Message}" });
            }
        }
        // GET: api/Classroom/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var classroom = await _service.GetByIdAsync(id);
                if (classroom == null)
                    return NotFound(new { error = "Class not found" });
                return Ok(classroom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving class: {ex.Message}" });
            }
        }

        // POST: api/Classroom
        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] Classroom classroom)
        {
            try
            {
                await _service.AddAsync(classroom);
                // Return the newly created course as JSON
                return CreatedAtAction(
                    nameof(GetClassById),
                    new { id = classroom.Id },
                    new { message = "Classroom added successfully!", classroom }
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

        // PUT: api/Classroom/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] Classroom classroom)
        {
            if (classroom.Id != id)
                classroom.Id = id;
            try
            {
                await _service.UpdateAsync(classroom);
                return Ok(new { message = "Classroom updated successfully!", classroom });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // DELETE: api/Classroom/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCLass(int id)
        {
            try
            {
                await _service.RemoveAsync(id);
                return Ok(new { message = "CLass deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
