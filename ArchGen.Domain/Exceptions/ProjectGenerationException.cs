namespace ArchGen.Domain.Exceptions;

public class ProjectGenerationException : DomainException
{
    public ProjectGenerationException(string message) : base(message)
    {
    }

    public ProjectGenerationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
