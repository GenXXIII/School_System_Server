namespace Application.DTOs.TeacherDTO;

public class TeacherCreateDto
{
    public string? TeacherId { get; set; }
    public string? FullName { get; set; }
    public int? CourseId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly HireDate { get; set; }
    public string? Password { get; set; }
}