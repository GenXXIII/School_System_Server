namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string? CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? Time { get; set; }
    public int Year { get; set; }
  
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
