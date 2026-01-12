using User.Api.Domain.Notifications;

namespace User.Api.Domain.ValueObjects;

public class Password : Notifiable
{
    public string Value { get; private set; }
    public Password(string password, bool isAlreadyHashed = false)
    {
        Value = password;
        if (!isAlreadyHashed) Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            AddNotification("Password", "Password cannot be empty!");
            return;
        }
        if (
            Value.Length < 8 || Value.Length > 32 ||
            !Value.Any(char.IsUpper) ||
            !Value.Any(char.IsLower) ||
            !Value.Any(char.IsDigit) ||
            !Value.Any(ch => !char.IsLetterOrDigit(ch))
            ) AddNotification("Password", "Invalid password format!");
    }

    public Password UpdatePassword(string password) => new(password);

}
