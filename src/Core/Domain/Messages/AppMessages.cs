using Domain.Abstractions;
using Domain.Resources;
using Microsoft.Extensions.Localization;

namespace Domain.Messages;

public static class AppMessages
{
    private static IStringLocalizer<SharedResource> _localizer = StringLocalizer._localizer;

    public static string ContactDelivered =>_localizer["ContactDelivered"].Value;
}
