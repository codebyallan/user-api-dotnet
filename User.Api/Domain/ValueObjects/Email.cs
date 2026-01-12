using System.Net.Mail;
using User.Api.Domain.Notifications;

namespace User.Api.Domain.ValueObjects;

public class Email : Notifiable
{
    public string Address { get; private set; }
    public Email(string address)
    {
        Address = Parse(address);
    }
    private string Parse(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            AddNotification("Email", "Empty email address!");
            return string.Empty;
        }
        try
        {
            var m = new MailAddress(address);
            return m.Address.Trim().ToLower();
        }
        catch (FormatException)
        {
            AddNotification("Email", "Invalid email address format!");
            return string.Empty;
        }
    }
    public Email UpdateEmail(string address) => new(address);
}
