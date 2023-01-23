using System.Net;

namespace GCPMicroservice.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode ApiCode { get; set; }
    public string ApiMessage { get; set; }

    public ApiException(HttpStatusCode apiCode, string apiMessage) : this(apiCode, apiMessage, null)
    {
    }

    public ApiException(HttpStatusCode apiCode, string apiMessage, string? message) : this(apiCode, apiMessage, message, null)
    {
    }

    public ApiException(HttpStatusCode apiCode, string apiMessage, string? message, Exception? inner) : base(message, inner)
    {
        ApiCode = apiCode;
        ApiMessage = apiMessage;
    }
}
