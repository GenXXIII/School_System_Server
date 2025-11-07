using Domain.Entities;
namespace Application.Interfaces.IServices;

public interface IGradeServices
{
    Task<IEnumerable<Grade>> GetAllAsync();
    Task<Grade?> GetByIdAsync(int id);
    Task AddAsync(Grade grade);
    Task UpdateAsync(Grade grade);
    Task RemoveAsync(int id);
}