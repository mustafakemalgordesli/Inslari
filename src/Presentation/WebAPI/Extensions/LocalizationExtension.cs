using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace WebAPI.Extensions;

public static class LocalizationExtension
{
    public static void ConfigureLocalization(this IServiceCollection services)
    {

        var supportedCultures = LocalizationConfig.GetSupportedCultures();

        services.AddLocalization(opt => opt.ResourcesPath = "");

        services.Configure<RequestLocalizationOptions>(opt =>
        {
            opt.DefaultRequestCulture = new RequestCulture("en");
            opt.SupportedUICultures = supportedCultures;
            opt.SupportedCultures = supportedCultures;
            opt.FallBackToParentCultures = true;
            opt.FallBackToParentUICultures = true;
            opt.RequestCultureProviders = new List<IRequestCultureProvider>()
            {
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });
    }
}

public class CustomRequestCultureProvider : RequestCultureProvider
{
    public CustomRequestCultureProvider() { }

    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var lang = httpContext.Request.Headers["culture"].ToString();

        Console.WriteLine(lang);

        if (!string.IsNullOrEmpty(lang))
        {
            return Task.FromResult(new ProviderCultureResult(lang, lang));
        }

        return Task.FromResult(new ProviderCultureResult("en", "en"));
    }
}

public static class LocalizationConfig
{
    public static CultureInfo[] GetSupportedCultures()
    {
        return
        [
            new CultureInfo("en"),
            new CultureInfo("tr")
        ];
    }
    public static RequestLocalizationOptions GetLocalizationOptions()
    {
        var supportedCultures = GetSupportedCultures();

        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        options.RequestCultureProviders.Insert(0, new WebAPI.Extensions.CustomRequestCultureProvider());

        return options;
    }
}