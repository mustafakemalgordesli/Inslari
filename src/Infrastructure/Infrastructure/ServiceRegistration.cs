﻿using Application.Abstractions;
using Domain.Options;
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

        EmailConfigurationOptions emailSettings = configuration.GetSection("EmailConfiguration").Get<EmailConfigurationOptions>() ?? throw new ArgumentException("Email configuration must be empty");

        services.AddFluentEmail(emailSettings.From)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettings.SmtpServer, emailSettings.Port, emailSettings.UserName, emailSettings.Password);

        services.AddScoped<IMailService, MailService>();
        services.AddScoped(typeof(IMailService<>), typeof(MailService<>));
    }
}
