namespace Application.DTOs.DepartmentDTO;

public class DepartmentUpdateDto
{
    public int Id { get; set; }
    public string? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int RoomNumber { get; set; }
    public string? Building { get; set; }
}
