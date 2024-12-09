using Domain.Options;
using Infrastructure.EventConsumers;
using MassTransit;

namespace WebAPI.Extensions;

public static class MassTransitExtension
{
    public static void CConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
         {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingInMemory((context, config) => config.ConfigureEndpoints(context));

            busConfigurator.AddConsumer<UserRegisteredConsumer>();

            //busConfigurator.UsingRabbitMq((context, config) =>
            //{
            //    var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();

            //    config.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHost, h =>
            //    {
            //        h.Username(rabbitMqOptions.UserName);
            //        h.Password(rabbitMqOptions.Password);
            //    });

            //    config.ConfigureEndpoints(context);
            //});
         });
    }
}
