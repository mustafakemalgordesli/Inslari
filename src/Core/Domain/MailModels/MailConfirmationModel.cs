namespace Domain.MailTemplates;

public class MailConfirmationModel
{
    public string Username { get; set; }
    public string VerificationLink { get; set; } = "https://example.com/verify?token=abc123"; 
    public string ExpirationTime { get; set; } = "24 saat"; 
    public string SupportEmail { get; set; } = "destek@ornek.com";
    public string CompanyName { get; set; } = "Şirket Adı";
}
