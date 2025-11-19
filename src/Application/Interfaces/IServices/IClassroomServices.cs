using Application.DTOs.ClassroomDTO;

namespace Application.Interfaces.IServices;

public interface IClassroomServices
{
    Task<IEnumerable<ClassroomDto>> GetAllAsync();
    Task<ClassroomDto?> GetByIdAsync(int id);
    Task AddAsync(ClassroomCreateDto dto);
    Task UpdateAsync(int id, ClassroomUpdateDto dto);
    Task RemoveAsync(int id);
}