using System.Globalization;
using System.Text.RegularExpressions;
using Application.Abstractions;
using Domain.DomainEvents;
using Domain.MailTemplates;
using Domain.Resources;
using MassTransit;
using Microsoft.Extensions.Localization;

namespace Infrastructure.EventConsumers;

public class UserRegisteredConsumer(IMailService<MailConfirmationModel> mailService, IStringLocalizer<SharedResource> localizer) : IConsumer<UserRegisteredDomainEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredDomainEvent> context)
    {
        var culture = "tr";
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        var res = await mailService.SendUsingTemplate(new EmailMetadata(context.Message.Email, localizer["MailConfirmation"].Value), new MailConfirmationModel(localizer, culture, "mustafa"), "MailConfirmationTemplate.cshtml");
    }
}
