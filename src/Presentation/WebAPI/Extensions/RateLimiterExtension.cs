using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Extensions;

public static class RateLimiterExtension
{
    public static void ConfigureRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>((httpContext) =>
            {
                var route = httpContext.GetEndpoint()?.DisplayName ?? "default";
                var partitionKey = $"{route}:{httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString()}";

                return RateLimitPartition.GetFixedWindowLimiter(
                       partitionKey: partitionKey,
                       factory: partition => new FixedWindowRateLimiterOptions
                       {
                           AutoReplenishment = true,
                           PermitLimit = 10,
                           Window = TimeSpan.FromMinutes(1)
                       });
            });

            options.AddFixedWindowLimiter("Fixed", config =>
            {
                config.PermitLimit = 5;
                config.Window = TimeSpan.FromSeconds(10);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 2;
            });
        });
    }
}
