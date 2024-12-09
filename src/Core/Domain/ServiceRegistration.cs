using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class ServiceRegistration
{
    public static void AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailConfigurationOptions>(configuration.GetSection("EmailConfiguration"));
    }
}
