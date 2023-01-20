namespace GCPMicroservice.Exceptions;

public class DataObjectNotFoundException : Exception
{
    public DataObjectNotFoundException()
    {

    }

    public DataObjectNotFoundException(string message)
        : base(message)
    {

    }

    public DataObjectNotFoundException(string message, Exception inner)
        : base(message, inner)
    {

    }
}
