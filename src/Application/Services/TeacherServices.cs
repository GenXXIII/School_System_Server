using Application.DTOs.TeacherDTO;
using Application.DTOs.Auth;
using FluentValidation;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
public class TeacherServices : ITeacherServices
{
    private readonly ITeacherRepositories _repo;
    private readonly IAuthService _authService;

    public TeacherServices(ITeacherRepositories repo, IAuthService authService)
    {
        _repo = repo;
        _authService = authService;
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
        if (dto.TeacherId != null && !await _repo.IsTeacherIdUniqueAsync(dto.TeacherId))
            throw new ValidationException("Teacher ID already exists.");

        // Register user if password is provided
        if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.Email))
        {
            var registerRequest = new RegisterRequest(
                dto.Email,
                dto.Password,
                "Teacher"
            );
            await _authService.RegisterAsync(registerRequest);
        }

        var teacher = dto.Adapt<Teacher>();
        await _repo.AddAsync(teacher);
    }

    public async Task UpdateAsync(int id, TeacherUpdateDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Teacher with ID {id} not found.");

        if (!string.IsNullOrEmpty(dto.TeacherId) && dto.TeacherId != existing.TeacherId)
        {
            if (!await _repo.IsTeacherIdUniqueAsync(dto.TeacherId!))
                throw new ValidationException("Teacher ID already exists.");
        }

        var teacher = dto.Adapt<Teacher>();
        teacher.Id = id;
        await _repo.UpdateAsync(teacher);
    }

    public async Task RemoveAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Teacher with ID {id} not found.");

        await _repo.RemoveAsync(id);
    }
}