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
        // var assembly = typeof(OrganizationController).GetTypeInfo().Assembly;
        // services.AddMvc()
        //     .AddApplicationPart(assembly);
        services.AddControllers();
        
        services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wishicoulderaseitmakeyourheartbelieve"))
                };
            });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Appointment Scheduling Service API",
                Description = "An ASP.NET Core Web API for scheduling appointments.",
            });
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilename = $"{typeof(OrganizationController).Assembly.GetName().Name}.xml";

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=6538;Database=postgres;Username=postgres;Password=postgres")
        );

        List<Profile> profiles = new List<Profile>()
        {
            new OrganizationMapping(), 
            new PlaceMapping(),
            new SlotMapping(),
            new RoleMapping(),
            new UserMapping(),
            new AppointmentMapping(),
        };
        services.AddAutoMapper(c => c.AddProfiles(profiles));
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IJwtUtil, JwtUtil>();
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
