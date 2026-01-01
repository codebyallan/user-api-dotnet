using System.Net.Mail;

namespace User.Api.Domain.ValueObjects;

public record Email
{
    public string EmailAddress { get; init; }
    public Email(string emailAddress)
    {
        EmailAddress = Parse(emailAddress);
    }
    private static string Parse(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress)) throw new ArgumentException("Invalid email address", nameof(emailAddress));
        try
        {
            var m = new MailAddress(emailAddress);
            return m.Address.Trim().ToLower();
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid email address format", nameof(emailAddress));
        }
    }
    public Email UpdateEmail(string emailAddress)
    {
        return this with
        {
            EmailAddress = Parse(emailAddress)
        };
    }
}
