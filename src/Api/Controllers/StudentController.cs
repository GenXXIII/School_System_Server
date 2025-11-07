using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _service;

        public StudentController(IStudentServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _service.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var students = await _service.GetByIdAsync(id);
            if (students ==null)
            {
                return NotFound("Students is not found");
            }

            return Ok(students);
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            await _service.AddAsync(student);
            return Ok("Student added successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student student)
        {
            await _service.UpdateAsync(student);
            return Ok("Student updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Student deleted successfully!");
        }
    }
}