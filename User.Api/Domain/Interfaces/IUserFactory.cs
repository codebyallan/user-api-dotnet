namespace User.Api.Domain.Interfaces;

public interface IUserFactory
{
    Entities.User Create(string firstName, string lastName, string email, string password);
}
