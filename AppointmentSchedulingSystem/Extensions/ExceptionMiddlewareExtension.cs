using Middlewares;

namespace AppointmentSchedulingSystem.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)  
    {  
        app.UseMiddleware<MyExceptionMiddleware>();  
    }  
}