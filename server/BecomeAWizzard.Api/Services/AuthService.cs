using BecomeAWizzard.Api.Data;
using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BecomeAWizzard.Api.Services;

public class AuthService(AppDbContext db)
{
    private readonly PasswordHasher<User> _hasher = new();

    public async Task<User> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(user => user.Email == email))
            throw new InvalidOperationException("E-postadressen er allerede registrert.");

        var user = new User { DisplayName = request.DisplayName.Trim(), Email = email };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> ValidateAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(candidate => candidate.Email == email);
        if (user is null) return null;
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }
}

