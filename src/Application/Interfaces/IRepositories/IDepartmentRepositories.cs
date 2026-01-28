using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IDepartmentRepositories
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task RemoveAsync(int id);
    Task<bool> IsDepartmentNameUniqueAsync(string departmentName);
}
