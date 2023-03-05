using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;

namespace application_infrastructure.TokenService;

public class TokenService: ITokenService
{
    private const int ExpirationMinutes = 30;
    
    /// <summary>
    /// Create a JWT token with the user's claims, the signing credentials, and the expiration time
    /// </summary>
    /// <param name="IdentityUser">The user object that contains the user's claims.</param>
    /// <returns>
    /// A JWT token
    /// </returns>
    public string CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Create a JWT token with the given claims, credentials and expiration date
    /// </summary>
    /// <param name="claims">A list of claims that will be added to the JWT token.</param>
    /// <param name="SigningCredentials">This is the key that will be used to sign the token.</param>
    /// <param name="DateTime">The expiration date of the token.</param>
    public JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            "creditSwisseAuth",
            "creditSwisseAuth",
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    /// <summary>
    /// It creates a list of claims for the user
    /// </summary>
    /// <param name="IdentityUser">This is the user object that is returned from the database.</param>
    /// <returns>
    /// A list of claims.
    /// </returns>
    public List<Claim> CreateClaims(IdentityUser user)
    {
        try
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheCreditSwisseAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// It creates a new signing credential using a symmetric security key that is created from a string
    /// </summary>
    /// <returns>
    /// A SigningCredentials object.
    /// </returns>
    public SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("!creditSwisseAuth!")
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}

