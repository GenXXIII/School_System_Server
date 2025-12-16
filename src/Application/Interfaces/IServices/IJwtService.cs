namespace Application.Interfaces.IServices;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string? role);
}