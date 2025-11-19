namespace Application.DTOs.StudentDTO;

public class StudentDto
{
    public int Id { get; set; }
    public string? StudentId { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}