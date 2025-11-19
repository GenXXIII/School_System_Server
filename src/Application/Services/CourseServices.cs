using Application.DTOs.CourseDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;

public class CourseServices : ICourseServices
{
    private readonly ICourseRepositories _repo;

    public CourseServices(ICourseRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _repo.GetAllAsync();
        return courses.Adapt<IEnumerable<CourseDto>>();
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _repo.GetByIdAsync(id);
        return course?.Adapt<CourseDto>();
    }

    public async Task AddAsync(CourseCreateDto dto)
    {
        var course = dto.Adapt<Course>();
        await _repo.AddAsync(course);
    }

    public async Task UpdateAsync(int id, CourseUpdateDto dto)
    {
        var course = dto.Adapt<Course>();
        course.Id = id;
        await _repo.UpdateAsync(course);
    }

    public async Task RemoveAsync(int id)
    {
        await _repo.RemoveAsync(id);
    }
}