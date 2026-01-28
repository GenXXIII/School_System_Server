using Application.DTOs.CourseDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;
using FluentValidation;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
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
        if (!await _repo.IsCourseIdUniqueAsync(dto.CourseId!))
            throw new ValidationException("Course ID already exists.");

        var course = dto.Adapt<Course>();
        await _repo.AddAsync(course);
    }

    public async Task UpdateAsync(int id, CourseUpdateDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Course with ID {id} not found.");

        if (!string.IsNullOrEmpty(dto.CourseId) && dto.CourseId != existing.CourseId)
        {
            if (!await _repo.IsCourseIdUniqueAsync(dto.CourseId!))
                throw new ValidationException("Course ID already exists.");
        }

        var course = dto.Adapt<Course>();
        course.Id = id;
        await _repo.UpdateAsync(course);
    }

    public async Task RemoveAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Course with ID {id} not found.");

        await _repo.RemoveAsync(id);
    }
}