using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ICourseRepositories
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task RemoveAsync(int id);
    Task<bool> IsCourseIdUniqueAsync(string courseId);
}