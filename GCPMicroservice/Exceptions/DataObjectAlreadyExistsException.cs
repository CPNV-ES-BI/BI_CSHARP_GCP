namespace GCPMicroservice.Exceptions;

public class DataObjectAlreadyExistsException : Exception
{
    public DataObjectAlreadyExistsException()
    {

    }

    public DataObjectAlreadyExistsException(string message)
        : base(message)
    {

    }

    public DataObjectAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {

    }
}
