using Microsoft.AspNetCore.Identity;
using User.Api.Application.Interfaces;

namespace User.Api.Application.Services;

public class HashPasswordService(IPasswordHasher<Domain.Entities.User> passwordHasher) : IHashPasswordService
{
    public string HashPassword(Domain.Entities.User user, string password)
    {
        return passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(Domain.Entities.User user, string hashed, string password)
    {
        ;
        return passwordHasher.VerifyHashedPassword(user, hashed, password) != PasswordVerificationResult.Failed;
    }
}
