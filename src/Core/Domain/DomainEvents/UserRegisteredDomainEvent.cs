using Domain.Common;

namespace Domain.DomainEvents;

public sealed record UserRegisteredDomainEvent(Guid Id, string Email) : IDomainEvent
{
}
