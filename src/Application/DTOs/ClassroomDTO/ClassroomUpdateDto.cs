namespace Application.DTOs.ClassroomDTO;

public class ClassroomUpdateDto
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public string? Classname { get; set; }
    public int RoomNumber { get; set; }
    public string? Building { get; set; }
}