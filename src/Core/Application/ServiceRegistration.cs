using System.Reflection;
using Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalPipelineBehavior<,>));
    }
}
