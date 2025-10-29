using System.Security.Claims;

namespace MNG.Application.Abstractions;
public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpinredToken(string token);
}
