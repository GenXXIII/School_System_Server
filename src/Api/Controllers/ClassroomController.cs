using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;

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

        [HttpGet] 
        public async Task<IActionResult> GetAllClassroom()
        {
            var classrooms = await _service.GetAllAsync();
            return Ok(classrooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroomById(int id)
        {
            var classrooms = await _service.GetByIdAsync(id);
            if (classrooms ==null)
            {
                return NotFound("Classroom is not found");
            }

            return Ok(classrooms);
        }
        [HttpPost]
        public async Task<IActionResult> AddClassroom([FromBody] Classroom classroom)
        {
            await _service.AddAsync(classroom);
            return Ok("Classroom added successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassroom([FromBody] Classroom classroom)
        {
            await _service.UpdateAsync(classroom);
            return Ok("Classroom updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Classroom deleted successfully!");
        }
    }
}