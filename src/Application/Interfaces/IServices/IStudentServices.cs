using Application.DTOs.StudentDTO;

namespace Application.Interfaces.IServices;

public interface IStudentServices
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task AddAsync(StudentCreateDto dto);
    Task UpdateAsync(int id, StudentUpdateDto dto);
    Task RemoveAsync(int id);
}