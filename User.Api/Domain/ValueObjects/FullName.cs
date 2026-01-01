namespace User.Api.Domain.ValueObjects;

public record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public FullName(string firstName, string lastName)
    {
        Validate(firstName, lastName);
        FirstName = firstName;
        LastName = lastName;
    }

    private static void Validate(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be empty", nameof(lastName));
    }

    public FullName UpdateFirstName(string firstName)
    {
        Validate(firstName, LastName);
        return this with { FirstName = firstName.Trim() };
    }

    public FullName UpdateLastName(string lastName)
    {
        Validate(FirstName, lastName);
        return this with { LastName = lastName.Trim() };
    }
}
