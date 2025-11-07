using Application.Interfaces.IServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;

namespace Application.Services;

public class TeacherServices : ITeacherServices
{
    private readonly ITeacherRepositories _repo;

    public TeacherServices(ITeacherRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Teacher?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(Teacher teacher) => await _repo.AddAsync(teacher);
    public async Task UpdateAsync(Teacher teacher) => await _repo.UpdateAsync(teacher);
    public async Task RemoveAsync(int id) => await _repo.RemoveAsync(id);
}