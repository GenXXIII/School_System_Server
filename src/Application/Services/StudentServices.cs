using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class StudentServices : IStudentServices
{
    private readonly IStudentRepositories _repo;

    public StudentServices(IStudentRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Student>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Student?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(Student student) => await _repo.AddAsync(student);
    public async Task UpdateAsync(int id,Student student) => await _repo.UpdateAsync(id,student);
    public async Task RemoveAsync(int id) => await _repo.RemoveAsync(id);
}