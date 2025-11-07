using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services;

public class GradeServices : IGradeServices
{
    private readonly IGradeRepositories _repo;

    public GradeServices(IGradeRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Grade?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(Grade grade) => await _repo.AddAsync(grade);
    public async Task UpdateAsync(Grade grade ) => await _repo.UpdateAsync(grade);
    public async Task RemoveAsync(int id) => await _repo.RemoveAsync(id);
}