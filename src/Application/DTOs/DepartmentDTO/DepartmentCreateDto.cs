namespace Application.DTOs.DepartmentDTO;

public class DepartmentCreateDto
{
    public string? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int RoomNumber { get; set; }
    public string? Building { get; set; }
}
