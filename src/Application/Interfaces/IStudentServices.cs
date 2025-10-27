using Domain.Entities;

namespace Application.Interfaces;

public interface IStudentServices
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task AddAsync(Student student);
    Task UpdateAsync(int id ,Student student);
    Task RemoveAsync(int id);
}