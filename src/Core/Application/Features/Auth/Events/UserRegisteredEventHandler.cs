using Domain.DomainEvents;
using MassTransit;
using MediatR;

namespace Application.Features.Auth.Events;

public sealed class UserRegisteredEventHandler(IBus bus) : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        await bus.Publish(notification, cancellationToken);
    }
}
