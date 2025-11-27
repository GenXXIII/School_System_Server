using Application.DTOs.ClassroomDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
public class ClassroomServices : IClassroomServices
{
    private readonly IClassroomRepositories _repo;
    public ClassroomServices(IClassroomRepositories repo)
    {
        _repo = repo;
    }
    public async Task<IEnumerable<ClassroomDto>> GetAllAsync()
    {
        var classrooms = await _repo.GetAllAsync();
        return classrooms.Adapt<IEnumerable<ClassroomDto>>();
    }

    public async Task<ClassroomDto?> GetByIdAsync(int id)
    {
        var classroom = await _repo.GetByIdAsync(id);
        return classroom?.Adapt<ClassroomDto>();
    }

    public async Task AddAsync(ClassroomCreateDto dto)
    {
        var classroom = dto.Adapt<Classroom>();
        await _repo.AddAsync(classroom);
    }

    public async Task UpdateAsync(int id, ClassroomUpdateDto dto)
    {
        var classroom = dto.Adapt<Classroom>();
        classroom.Id = id;
        await _repo.UpdateAsync(classroom);
    }

    public async Task RemoveAsync(int id)
    {
        await _repo.RemoveAsync(id);
    }
}