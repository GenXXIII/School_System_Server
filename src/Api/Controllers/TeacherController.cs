using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;

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

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _service.GetAllAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teachers = await _service.GetByIdAsync(id);
            if (teachers ==null)
            {
                return NotFound("Teacher is not found");
            }

            return Ok(teachers);
        }
        [HttpPost]
        public async Task<IActionResult> AddTeacher([FromBody] Teacher teacher)
        {
            await _service.AddAsync(teacher);
            return Ok("Student added successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher([FromBody] Teacher teacher)
        {
            await _service.UpdateAsync(teacher);
            return Ok("Student updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Student deleted successfully!");
        }
    }
}