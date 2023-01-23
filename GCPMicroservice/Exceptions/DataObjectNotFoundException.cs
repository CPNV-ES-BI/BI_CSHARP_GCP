using System.Net;

namespace GCPMicroservice.Exceptions;

public class DataObjectNotFoundException : ApiException
{
    private const HttpStatusCode HttpCode = HttpStatusCode.NotFound;
    private const string HttpMessage = "Data object not found";

    public DataObjectNotFoundException() : base(HttpCode, HttpMessage)
    {
    }

    public DataObjectNotFoundException(string message) : base(HttpCode, HttpMessage, message)
    {
    }

    public DataObjectNotFoundException(string message, Exception? inner)
        : base(HttpCode, HttpMessage, message, inner)
    {

    }
}
