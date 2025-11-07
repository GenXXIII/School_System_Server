using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services;

public class ClassroomServices :IClassroomServices
{
    private readonly IClassroomRepositories _repo;

    public ClassroomServices(IClassroomRepositories repo)
    {
        _repo = repo;
    }
    public async Task<IEnumerable<Classroom>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Classroom?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(Classroom classroom) => await _repo.AddAsync(classroom);
    public async Task UpdateAsync(Classroom classroom) => await _repo.UpdateAsync(classroom);
    public async Task RemoveAsync(int id) => await _repo.RemoveAsync(id);
}