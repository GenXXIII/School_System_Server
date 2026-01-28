namespace Application.DTOs.StudentDTO;

public class StudentCreateDto
{
    public string? StudentId { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int Year { get; set; }
    public int? DepartmentId { get; set; }
    public string? Password { get; set; }
}
