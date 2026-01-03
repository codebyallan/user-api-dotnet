using User.Api.Domain.Notifications;

namespace User.Api.Domain.ValueObjects;

public class FullName : Notifiable
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            AddNotification(nameof(FirstName), "First name cannot be empty!");

        if (string.IsNullOrWhiteSpace(LastName))
            AddNotification(nameof(LastName), "Last name cannot be empty!");
    }

    public FullName UpdateFirstName(string firstName)
    {
        return new FullName(firstName, LastName);
    }

    public FullName UpdateLastName(string lastName)
    {
        return new FullName(FirstName, lastName);
    }
}
