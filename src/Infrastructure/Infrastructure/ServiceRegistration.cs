using Application.Abstractions;
using Domain.Common;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));

        EmailConfiguration emailSettings = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>() ?? throw new ArgumentException("Email configuration must be empty");

        services.AddFluentEmail(emailSettings.From)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettings.SmtpServer, emailSettings.Port, emailSettings.UserName, emailSettings.Password);

        services.AddScoped<IMailService, MailService>();
        services.AddScoped(typeof(IMailService<>), typeof(MailService<>));
    }
}
