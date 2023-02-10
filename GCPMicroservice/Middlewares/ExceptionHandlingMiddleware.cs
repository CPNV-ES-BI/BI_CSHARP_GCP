using GCPMicroservice.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace GCPMicroservice.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode code = HttpStatusCode.InternalServerError;
        string message = "";

        if (exception is ApiException apiException)
        {
            code = apiException.ApiCode;
            message = apiException.ApiMessage;
        }

        string result = JsonConvert.SerializeObject(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        
        return context.Response.WriteAsync(result);
    }
}
