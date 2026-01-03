namespace User.Api.Application.Interfaces;

public interface IHashPasswordService
{
    string HashPassword(Domain.Entities.User user, string password);
    bool VerifyPassword(Domain.Entities.User user, string hashed, string password);
}
