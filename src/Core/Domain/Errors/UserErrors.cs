using Domain.Abstractions;
using Domain.Resources;
using Microsoft.Extensions.Localization;

namespace Domain.Errors;

public static class UserErrors
{
    private static IStringLocalizer<SharedResource> _localizer = StringLocalizer._localizer;

    public static Error InvalidUsername => Error.Failure("User.InvalidUsername", _localizer["User.InvalidUsername"].Value);
    public static Error UserExist => Error.Conflict("User.Exists", _localizer["User.Exists"].Value);
    public static Error UserNotFound => Error.NotFound("User.NotFound", _localizer["User.NotFound"].Value);
    public static Error PasswordWrong => Error.Conflict("User.PasswordWrong", _localizer["User.PasswordWrong"].Value);
}
