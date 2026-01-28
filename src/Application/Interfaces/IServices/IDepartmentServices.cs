using Application.DTOs.DepartmentDTO;

namespace Application.Interfaces.IServices;

public interface IDepartmentServices
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task AddAsync(DepartmentCreateDto dto);
    Task UpdateAsync(int id, DepartmentUpdateDto dto);
    Task RemoveAsync(int id);
}
