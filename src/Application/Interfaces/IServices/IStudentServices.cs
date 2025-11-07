using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IStudentServices
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task RemoveAsync(int id);
}