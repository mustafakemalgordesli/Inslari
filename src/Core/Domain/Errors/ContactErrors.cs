using Domain.Abstractions;
using Domain.Resources;
using Microsoft.Extensions.Localization;

namespace Domain.Errors;

public static class ContactErrors
{
    private static IStringLocalizer<SharedResource> _localizer = StringLocalizer._localizer;

    public static Error MessageNotDelivered => Error.Failure("Contact.MessageNotDelivered", _localizer["Contact.MessageNotDelivered"].Value);
}
