namespace User.Api.Domain.ValueObjects;

public record Password
{
    public string Value { get; init; }

    public Password(string password)
    {
        Validate(password);
        Value = password;
    }

    private static void Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be empty", nameof(password));
        if (
            password.Length < 8 || password.Length > 32 ||
            !password.Any(char.IsUpper) ||
            !password.Any(char.IsLower) ||
            !password.Any(char.IsDigit) ||
            !password.Any(ch => !char.IsLetterOrDigit(ch))
            )
        {
            throw new ArgumentException("Invalid password format", nameof(password));
        }
    }

    public Password UpdatePassword(string newPassword)
    {
        Validate(newPassword);
        return this with { Value = newPassword };
    }
}
