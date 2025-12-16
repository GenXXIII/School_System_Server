using System.ComponentModel.DataAnnotations;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly IAppDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IAppDbContext context,
        PasswordService passwordService,
        IJwtService jwtService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }
    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(x => x.Email == request.Email))
            throw new ValidationException("Email already exists");

        var user = new User
        {
            Email = request.Email,
            Role = request.Role
        };

        user.PasswordHash = _passwordService.Hash(request.Password, user);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }


    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
                       .FirstOrDefaultAsync(x => x.Email == request.Email)
                   ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!_passwordService.Verify(user.PasswordHash, request.Password, user))
            throw new UnauthorizedAccessException("Invalid credentials");

        return _jwtService.GenerateToken(user.Id, user.Email, user.Role);
    }
}