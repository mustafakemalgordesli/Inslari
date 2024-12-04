namespace Application.Abstractions;

public interface IMailService
{
    Task<bool> Send(EmailMetadata metadata);
}

public class EmailMetadata(string toAdress, string subject, string? body = "", string? attachmentPath = "")
{
    public string ToAddress { get; set; } = toAdress;
    public string Subject { get; set; } = subject;
    public string? Body { get; set; } = body;
    public string? AttachmentPath { get; set; } = attachmentPath;
}