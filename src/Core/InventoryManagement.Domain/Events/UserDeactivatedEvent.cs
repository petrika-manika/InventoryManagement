namespace InventoryManagement.Domain.Events;

public sealed class UserDeactivatedEvent : IEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public DateTime OccurredOn { get; }

    public UserDeactivatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
        OccurredOn = DateTime.UtcNow;
    }
}
