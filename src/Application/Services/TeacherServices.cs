using Application.DTOs.TeacherDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;

public class TeacherServices : ITeacherServices
{
    private readonly ITeacherRepositories _repo;

    public TeacherServices(ITeacherRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllAsync()
    {
        var teachers = await _repo.GetAllAsync();
        return teachers.Adapt<IEnumerable<TeacherDto>>();
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await _repo.GetByIdAsync(id);
        return teacher?.Adapt<TeacherDto>();
    }

    public async Task AddAsync(TeacherCreateDto dto)
    {
        var teacher = dto.Adapt<Teacher>();
        await _repo.AddAsync(teacher);
    }

    public async Task UpdateAsync(int id, TeacherUpdateDto dto)
    {
        var teacher = dto.Adapt<Teacher>();
        teacher.Id = id;
        await _repo.UpdateAsync(teacher);
    }

    public async Task RemoveAsync(int id)
    {
        await _repo.RemoveAsync(id);
    }
}