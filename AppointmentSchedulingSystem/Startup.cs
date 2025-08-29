using System.Reflection;
using System.Text;
using AppointmentSchedulingSystem.Extensions;
using AutoMapper;
using Contracts;
using Contracts.Appointment;
using Contracts.Organization;
using Contracts.Security;
using Contracts.User;
using Controllers;
using Domain;
using Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Services;
using Utils;

namespace AppointmentSchedulingSystem;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("Here2");
        services.Configure<Settings>(Configuration.GetSection("Settings"));
        services.AddControllers();
        services.ConfigureAuthentication();
        services.AddSwagger();
        services.AddDbContextFactory();
        services.AddMapping();
        services.AddServices();
    }
    
    public void Configure(IApplicationBuilder app)
    {
        // if (app.Environment.IsDevelopment())
        // {
        
        app.UseExceptionMiddleware();  
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        // }
        Console.WriteLine("Here1");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints
            .MapControllers()
            .RequireAuthorization()
        );
    }
}
