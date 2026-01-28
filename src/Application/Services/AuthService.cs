using System;
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
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _context.Users.AnyAsync(x => x.Email == email))
            throw new ValidationException("Email already exists");

        var passwordHash = _passwordService.Hash(request.Password); // ✅ independent

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = request.Role ?? "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // ✅ Use correct Verify method
        if (!_passwordService.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.Role
        );

        return new LoginResponse(token);
    }
}