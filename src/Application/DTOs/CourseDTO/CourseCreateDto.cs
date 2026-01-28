namespace Application.DTOs.CourseDTO;

public class CourseCreateDto
{
    public string? CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? Time { get; set; }
    public int Year { get; set; }
    public int? DepartmentId { get; set; }
}
