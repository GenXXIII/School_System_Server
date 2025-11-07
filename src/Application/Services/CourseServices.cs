using Application.Interfaces.IServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;

namespace Application.Services;

public class CourseServices : ICourseServices
{
    private readonly ICourseRepositories _repo;

    public CourseServices(ICourseRepositories repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Course>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Course?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(Course course) => await _repo.AddAsync(course);
    public async Task UpdateAsync(Course course) => await _repo.UpdateAsync(course);
    public async Task RemoveAsync(int id) => await _repo.RemoveAsync(id);
}