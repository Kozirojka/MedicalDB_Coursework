using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MedicApp.Infrastructure.Services.GoogleMapsApi;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}