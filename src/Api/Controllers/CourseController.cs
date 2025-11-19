using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _service;

        public CourseController(ICourseServices service)
        {
            _service = service;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            try
            {
                var course = await _service.GetAllAsync();
                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving course: {ex.Message}" });
            }
        }
        // GET: api/Course/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _service.GetByIdAsync(id);
                if (course == null)
                    return NotFound(new { error = "Course not found" });
                return Ok(course); // JSON object
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving course: {ex.Message}" });
            }
        }

        // POST: api/Course
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            try
            {
                await _service.AddAsync(course);
                // Return the newly created course as JSON
                return CreatedAtAction(
                    nameof(GetCourseById),
                    new { id = course.Id },
                    new { message = "Course added successfully!", course }
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

        // PUT: api/Course/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (course.Id != id)
                course.Id = id;
            try
            {
                await _service.UpdateAsync(course);
                return Ok(new { message = "Course updated successfully!", course });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/Course/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _service.RemoveAsync(id);
                return Ok(new { message = "Course deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
