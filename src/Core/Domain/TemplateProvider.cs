namespace Domain;


public interface ITemplateProvider
{
    string GetTemplatePath(string templateName);
}

public class TemplateProvider : ITemplateProvider
{
    private readonly string _templateDirectory;

    public TemplateProvider(string templateDirectory)
    {
        _templateDirectory = templateDirectory;
    }

    public string GetTemplatePath(string templateName)
    {
        return Path.Combine(_templateDirectory, templateName);
    }
}