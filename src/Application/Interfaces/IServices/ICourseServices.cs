using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface ICourseServices
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task RemoveAsync(int id);
}