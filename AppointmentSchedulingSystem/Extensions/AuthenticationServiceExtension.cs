using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentSchedulingSystem.Extensions;

public static class AuthenticationServiceExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
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
                    ValidIssuer = "http://localhost:5002",
                    ValidAudience = "http://localhost:5002",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wishicoulderaseitmakeyourheartbelieve"))
                };
            });
    }
}