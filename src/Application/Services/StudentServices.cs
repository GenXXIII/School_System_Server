using Application.DTOs.StudentDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
public class StudentServices : IStudentServices
{
    private readonly IStudentRepositories _repo;

    public StudentServices(IStudentRepositories repo)
    {
        _repo = repo;
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
        var student = dto.Adapt<Student>();
        await _repo.AddAsync(student);
    }

    public async Task UpdateAsync(int id, StudentUpdateDto dto)
    {
        var student = dto.Adapt<Student>();
        student.Id = id;
        await _repo.UpdateAsync(student);
    }

    public async Task RemoveAsync(int id)
    {
        await _repo.RemoveAsync(id);
    }
}