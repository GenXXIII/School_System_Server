namespace Domain.Entities;

public class Classroom
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public string? Classname { get; set; }
    public int RoomNumber { get; set; }
    public string? Building { get; set; }
}