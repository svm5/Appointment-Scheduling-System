using System.Security.Claims;

namespace Contracts.Security;

public interface IJwtUtil
{
    string GenerateJwtToken(ICollection<Claim> claims);
}