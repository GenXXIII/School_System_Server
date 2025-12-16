using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<string> LoginAsync(LoginRequest request);
}

