using System.Runtime;
using Application.Abstractions;
using Domain;
using FluentEmail.Core;
using RazorLight;

namespace Infrastructure.Services;

public class MailService(IFluentEmail _fluentEmail, ITemplateProvider _templateProvider) : IMailService
{
    public async Task<bool> Send(EmailMetadata metadata)
    {
        var _fluent = _fluentEmail.To(metadata.ToAddress)
            .Subject(metadata.Subject);
            
        foreach(var path in metadata.AttachmentPathList)
        {
            if (!string.IsNullOrEmpty(path) || File.Exists(path))
            {
                _fluent.AttachFromFilename(_templateProvider.GetTemplatePath(path), attachmentName: path);
            }
        }    

        var res = await _fluent.Body(metadata.Body)
            .SendAsync();

        return res.Successful;
    }
}

public class MailService<TModel>(IFluentEmail _fluentEmail, ITemplateProvider _templateProvider) : MailService(_fluentEmail: _fluentEmail, _templateProvider: _templateProvider), IMailService<TModel> where TModel : class
{
    public async Task<bool> SendUsingTemplate(EmailMetadata metadata, TModel model, string templateName)
    {
        string templateFile = _templateProvider.GetTemplatePath(Path.Combine("MailTemplates", templateName));

        if (string.IsNullOrEmpty(templateFile) || !File.Exists(templateFile))
        {
            throw new FileNotFoundException($"Template file '{templateName}' not found.");
        }

        var _fluent = _fluentEmail.To(metadata.ToAddress)
            .Subject(metadata.Subject);

        foreach (var path in metadata.AttachmentPathList)
        {
            if (!string.IsNullOrEmpty(path) || File.Exists(path))
            {
                _fluent.AttachFromFilename(_templateProvider.GetTemplatePath(path), attachmentName: path);
            }
        }

        var res = await _fluent.UsingTemplateFromFile(templateFile, model)
            .SendAsync();

        return res.Successful;
    }
}
