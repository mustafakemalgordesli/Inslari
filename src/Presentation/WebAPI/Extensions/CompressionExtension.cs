namespace WebAPI.Extensions;

public static class CompressionExtension
{
    public static void ConfigureCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
         {
            options.EnableForHttps = true;
         });
    }
}
