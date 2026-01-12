using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Domain.Factories;

public class UserFactory : IUserFactory
{
    public Entities.User Create(string firstName, string lastName, string email, string password) => new(Guid.NewGuid(), new FullName(firstName, lastName), new Email(email), new Password(password));
}
