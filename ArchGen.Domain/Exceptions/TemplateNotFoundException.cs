namespace ArchGen.Domain.Exceptions;

public class TemplateNotFoundException : DomainException
{
    public TemplateNotFoundException(string templateName) 
        : base($"Template '{templateName}' não foi encontrado.")
    {
    }
}
