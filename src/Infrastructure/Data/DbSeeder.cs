using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, PasswordService passwordService)
    {
        if (!await context.Users.AnyAsync(u => u.Email == "genxxiii@gmail.com"))
        {
            var admin = new User
            {
                Email = "genxxiii@gmail.com",
                PasswordHash = passwordService.Hash("1234"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
