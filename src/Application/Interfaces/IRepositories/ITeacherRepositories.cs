using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ITeacherRepositories
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task AddAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task RemoveAsync(int id);
    Task<bool> IsTeacherIdUniqueAsync(string teacherId);
}