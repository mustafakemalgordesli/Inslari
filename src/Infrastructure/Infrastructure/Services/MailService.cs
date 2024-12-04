using Application.Abstractions;
using FluentEmail.Core;

namespace Infrastructure.Services;

public class MailService(IFluentEmail _fluentEmail) : IMailService
{
    public async Task<bool> Send(EmailMetadata metadata)
    {        
        var res = await _fluentEmail.To(metadata.ToAddress)
            .Subject(metadata.Subject)
            .Body(metadata.Body)
            .SendAsync();

        return res.Successful;
    }
}
