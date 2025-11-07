using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IClassroomServices
{
    Task<IEnumerable<Classroom>> GetAllAsync();
    Task<Classroom?> GetByIdAsync(int id);
    Task AddAsync(Classroom classroom);
    Task UpdateAsync(Classroom classroom);
    Task RemoveAsync(int id);
}