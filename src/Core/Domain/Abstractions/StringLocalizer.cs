using Domain.Resources;
using Microsoft.Extensions.Localization;

namespace Domain.Abstractions;

public static class StringLocalizer
{
    public static IStringLocalizer<SharedResource> _localizer;
    public static void Configure(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }
}

public static class StringExtensions
{
    public static string Format(this string str, params object[] parameters)
    {
        return string.Format(str, parameters);
    }
}
