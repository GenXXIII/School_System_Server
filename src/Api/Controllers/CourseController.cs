using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;

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

        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await _service.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var courses = await _service.GetByIdAsync(id);
            if (courses ==null)
            {
                return NotFound("Courses is not found");
            }

            return Ok(courses);
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            await _service.AddAsync(course);
            return Ok("Course added successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            await _service.UpdateAsync(course);
            return Ok("Course updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Course deleted successfully!");
        }
    }
}