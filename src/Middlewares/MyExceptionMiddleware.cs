using System.Net;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Middlewares;

public class MyExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public MyExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = (int)HttpStatusCode.InternalServerError;
        switch (exception.GetType().Name)
        {
            case nameof(NotFoundException):
                statusCode = (int)HttpStatusCode.NotFound;
                break;
            case nameof(PlaceIsBusyException):
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                break;
            case nameof(InvalidLoginCredentialsException):
                statusCode = (int)HttpStatusCode.Unauthorized;
                break;
        }
        
        var result = JsonConvert.SerializeObject(new  
        {  
            StatusCode = statusCode,  
            ErrorMessage = exception.Message  
        }); 
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        // await context.Response.WriteAsync(exception.Message);
        await context.Response.WriteAsync(result);
    }
}