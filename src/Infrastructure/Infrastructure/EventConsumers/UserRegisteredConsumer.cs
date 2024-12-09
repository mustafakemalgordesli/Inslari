using Domain.DomainEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EventConsumers;

public class UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredDomainEvent>
{
    public Task Consume(ConsumeContext<UserRegisteredDomainEvent> context)
    {
        Thread.Sleep(10000);
        logger.LogInformation("{Consumer}: {Message}", nameof(UserRegisteredConsumer), context.Message.Id);
        return Task.CompletedTask;
    }
}
