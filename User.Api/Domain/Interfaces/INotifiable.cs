using User.Api.Domain.Notifications;

namespace User.Api.Domain.Interfaces;

public interface INotifiable
{
    bool IsValid { get; }
    IReadOnlyCollection<NotificationMessage> Notifications { get; }
    void AddNotification(NotificationMessage message);
    void AddNotification(string key, string message);
    void AddNotifications(IReadOnlyCollection<NotificationMessage> notifications);
    void AddNotifications(INotifiable notifiable);
    public void ClearNotifications();
}
