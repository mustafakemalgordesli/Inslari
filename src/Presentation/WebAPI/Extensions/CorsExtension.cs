namespace WebAPI.Extensions;

public static class CorsExtension
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string MyAllowSpecificOrigins)
    {
        var allowedOrigins = configuration["CORS"]
            ?.Split(";", StringSplitOptions.RemoveEmptyEntries) 
            ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins(allowedOrigins)
                                  .AllowAnyMethod()
                                  .AllowCredentials()
                                  .AllowAnyHeader();
                              });
        });
    }
}
