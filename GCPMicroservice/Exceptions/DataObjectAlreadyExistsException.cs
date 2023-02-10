using System.Net;

namespace GCPMicroservice.Exceptions;

public class DataObjectAlreadyExistsException : ApiException
{
    private const HttpStatusCode HttpCode = HttpStatusCode.Conflict;
    private const string HttpMessage = "Data object already exists";

    public DataObjectAlreadyExistsException() : base(HttpCode, HttpMessage)
    {
    }
    
    public DataObjectAlreadyExistsException(string message) : base(HttpCode, HttpMessage, message)
    {
    }

    public DataObjectAlreadyExistsException(string message, Exception inner)
        : base(HttpCode, HttpMessage, message, inner)
    {

    }
}
