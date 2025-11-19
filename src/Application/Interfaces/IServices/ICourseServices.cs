using Application.DTOs.CourseDTO;

namespace Application.Interfaces.IServices;

public interface ICourseServices
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task AddAsync(CourseCreateDto dto);
    Task UpdateAsync(int id, CourseUpdateDto dto);
    Task RemoveAsync(int id);
}