using User.Api.Domain.Notifications;
using User.Api.Domain.ValueObjects;

namespace User.Api.Domain.Entities;

public class User : Notifiable
{
    public Guid Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email EmailAddress { get; private set; }
    public Password Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public User(Guid id, FullName fullName, Email emailAddress, Password password)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty!", nameof(id));
        Id = id;
        FullName = fullName;
        EmailAddress = emailAddress;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }
    public void ChangeFirstName(string firstName)
    {
        FullName = FullName.UpdateFirstName(firstName);
        UpdatedAt = DateTime.UtcNow;
        Validate();
    }

    public void ChangeLastName(string lastName)
    {
        FullName = FullName.UpdateLastName(lastName);
        UpdatedAt = DateTime.UtcNow;
        Validate();
    }

    public void ChangeEmailAddress(string emailAddress)
    {
        EmailAddress = EmailAddress.UpdateEmail(emailAddress);
        UpdatedAt = DateTime.UtcNow;
        Validate();
    }

    public void ChangePassword(string password)
    {
        Password = Password.UpdatePassword(password);
        UpdatedAt = DateTime.UtcNow;
        Validate();
    }

    public void ApplyPasswordHash(string hash)
    {
        Password = new Password(hash, isAlreadyHashed: true);
    }

    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }
    private void Validate()
    {
        ClearNotifications();
        AddNotifications(FullName);
        AddNotifications(EmailAddress);
        AddNotifications(Password);
    }

}
