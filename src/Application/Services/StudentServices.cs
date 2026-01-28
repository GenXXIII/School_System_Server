using Application.DTOs.StudentDTO;
using Application.DTOs.Auth;
using FluentValidation;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
public class StudentServices : IStudentServices
{
    private readonly IStudentRepositories _repo;
    private readonly IAuthService _authService;

    public StudentServices(IStudentRepositories repo, IAuthService authService)
    {
        _repo = repo;
        _authService = authService;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _repo.GetAllAsync();
        return students.Adapt<IEnumerable<StudentDto>>();
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _repo.GetByIdAsync(id);
        return student?.Adapt<StudentDto>();
    }

    public async Task AddAsync(StudentCreateDto dto)
    {
        if (dto.StudentId != null && !await _repo.IsStudentIdUniqueAsync(dto.StudentId))
            throw new ValidationException("Student ID already exists.");

        // Register user if password is provided
        if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.Email))
        {
            var registerRequest = new RegisterRequest(
                dto.Email,
                dto.Password,
                "Student"
            );
            await _authService.RegisterAsync(registerRequest);
        }

        var student = dto.Adapt<Student>();
        await _repo.AddAsync(student);
    }

    public async Task UpdateAsync(int id, StudentUpdateDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Student with ID {id} not found.");

        if (!string.IsNullOrEmpty(dto.StudentId) && dto.StudentId != existing.StudentId)
        {
            if (!await _repo.IsStudentIdUniqueAsync(dto.StudentId!))
                throw new ValidationException("Student ID already exists.");
        }

        var student = dto.Adapt<Student>();
        student.Id = id;
        await _repo.UpdateAsync(student);
    }

    public async Task RemoveAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Student with ID {id} not found.");

        await _repo.RemoveAsync(id);
    }
}