namespace Domain.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string? TeacherId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly HireDate { get; set; }
    
    // Foreign Key
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}