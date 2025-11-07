using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeServices _service;

        public GradeController(IGradeServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGrade()
        {
            var grades = await _service.GetAllAsync();
            return Ok(grades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var grades = await _service.GetByIdAsync(id);
            if (grades ==null)
            {
                return NotFound("Grade is not found");
            }

            return Ok(grades);
        }
        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            await _service.AddAsync(grade);
            return Ok("Grade added successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade([FromBody] Grade grade)
        {
            await _service.UpdateAsync(grade);
            return Ok("Grade updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Grade deleted successfully!");
        }
    }
}