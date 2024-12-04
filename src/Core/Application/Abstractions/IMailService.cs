namespace Application.Abstractions;

public interface IMailService
{
    Task<bool> Send(EmailMetadata metadata);
}

public interface IMailService<TModel> : IMailService where TModel : class
{
    Task<bool> SendUsingTemplate(EmailMetadata metadata,  TModel model, string templateName);
}

public record EmailMetadata(
    string ToAddress,
    string Subject,
    string? Body = "",
    List<string>? AttachmentPathList = null
)
{
    public List<string> AttachmentPathList { get; init; } = AttachmentPathList ?? new List<string>();
}