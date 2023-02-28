using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace application_infrastructure.TokenService;

public interface ITokenService
{
    string CreateToken(IdentityUser user);
    JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration);
    List<Claim> CreateClaims(IdentityUser user);
    SigningCredentials CreateSigningCredentials();
}
