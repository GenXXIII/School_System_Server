using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface ITeacherServices
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task AddAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task RemoveAsync(int id);
}