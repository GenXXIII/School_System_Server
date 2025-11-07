using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IStudentRepositories
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task RemoveAsync(int id);
}