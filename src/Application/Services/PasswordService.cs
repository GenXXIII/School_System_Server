using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class PasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
    {
        var user = new User(); // dummy user instance
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(string password, string hash)
    {
        var user = new User(); // same approach
        var result = _hasher.VerifyHashedPassword(user, hash, password);

        return result == PasswordVerificationResult.Success
               || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}