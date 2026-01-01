using User.Api.Domain.ValueObjects;

namespace User.Api.Domain.Entities;

public record User
{
    public Guid Id { get; init; }
    public FullName FullName { get; init; }
    public Email EmailAddress { get; init; }
    public Password Password { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }

    public User(Guid id, FullName fullName, Email emailAddress, Password password)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));
        Id = id;
        FullName = fullName;
        EmailAddress = emailAddress;
        Password = password;
        CreatedAt = DateTime.UtcNow;
    }

    public User ChangeFirstName(string firstName)
    {
        FullName newFullName = FullName.UpdateFirstName(firstName);
        return this with { FullName = newFullName, UpdatedAt = DateTime.UtcNow };
    }

    public User ChangeLastName(string lastName)
    {
        FullName newFullName = FullName.UpdateLastName(lastName);
        return this with { FullName = newFullName, UpdatedAt = DateTime.UtcNow };
    }

    public User ChangeEmailAddress(string emailAddress)
    {
        Email newEmailAddress = EmailAddress.UpdateEmail(emailAddress);
        return this with { EmailAddress = newEmailAddress, UpdatedAt = DateTime.UtcNow };
    }

    public User ChangePassword(string password)
    {
        Password newPassword = Password.UpdatePassword(password);
        return this with { Password = newPassword, UpdatedAt = DateTime.UtcNow };
    }

}
