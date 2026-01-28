namespace Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int RoomNumber { get; set; }
    public string? Building { get; set; }
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
