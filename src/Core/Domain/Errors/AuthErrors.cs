using Domain.Abstractions;
using Domain.Resources;
using Microsoft.Extensions.Localization;

namespace Domain.Errors;

public static class AuthErrors
{
    private static IStringLocalizer<SharedResource> _localizer = StringLocalizer._localizer;

    public static Error AuthError => Error.Failure("AuthError", _localizer["AuthError"].Value);
}
