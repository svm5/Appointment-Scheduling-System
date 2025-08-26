using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts.Security;
using Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Utils;

public class JwtUtil : IJwtUtil
{
    private readonly IOptions<Settings> _settings;

    public JwtUtil(IOptions<Settings> settings)
    {
        _settings = settings;
    }
    
    public string GenerateJwtToken(ICollection<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Secret));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: _settings.Value.Issuer,
            audience: _settings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }
}