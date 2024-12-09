using Domain.DomainEvents;
using MediatR;

namespace Application.Features.Auth.Events;

public sealed class UserRegisteredEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
