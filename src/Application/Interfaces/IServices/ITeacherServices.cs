using Application.DTOs.TeacherDTO;

namespace Application.Interfaces.IServices;

public interface ITeacherServices
{
    Task<IEnumerable<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task AddAsync(TeacherCreateDto dto);
    Task UpdateAsync(int id, TeacherUpdateDto dto);
    Task RemoveAsync(int id);
}