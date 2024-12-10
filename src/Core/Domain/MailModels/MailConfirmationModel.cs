using System.Globalization;
using System.Text.RegularExpressions;
using Domain.Abstractions;
using Microsoft.Extensions.Localization;

namespace Domain.MailTemplates;

public class MailConfirmationModel
{
    public MailConfirmationModel(IStringLocalizer localizer, string culture, string username)
    {
        Culture = culture;
        MailConfirmation = localizer["MailConfirmation"].Value;
        Hello = localizer["Hello"].Value;
        ThankForCreateAccount = localizer["ThankForCreateAccount"].Value;
        ExpirationTime = localizer["ExpirationTime"].Value;
        ValidTimeHtml = localizer["ValidTimeHtml"].Value.Format(ExpirationTime);
        ThanksMsg = localizer["ThanksMsg"].Value.Format(CompanyName);
        VerifyMyMail = localizer["VerifyMyMail"].Value;
        BtnWorkMsg = localizer["BtnWorkMsg"].Value;
        AccountMsg = localizer["AccountMsg"].Value;
        IgnoreMsg = localizer["IgnoreMsg"].Value;
    }
    public string IgnoreMsg { get; set; }
    public string AccountMsg { get; set; }
    public string BtnWorkMsg { get; set; }
    public string ValidTimeHtml {  get; set; }
    public string Culture { get; set; }
    public string ThankForCreateAccount { get; set; }
    public string Hello { get; set; }
    public string MailConfirmation { get; set; }
    public string Username { get; set; }
    public string VerificationLink { get; set; } = "https://example.com/verify?token=abc123"; 
    public string ExpirationTime { get; set; }
    public string SupportEmail { get; set; } = "info@inslari.com";
    public string CompanyName { get; set; } = "Inslari";
    public string ThanksMsg { get; set; }
    public string VerifyMyMail { get; set; }
}
