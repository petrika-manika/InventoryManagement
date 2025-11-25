namespace InventoryManagement.Domain.Events;

public interface IEvent
{
    DateTime OccurredOn { get; }
}
