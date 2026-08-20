namespace ArchGen.Domain;

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }
}
