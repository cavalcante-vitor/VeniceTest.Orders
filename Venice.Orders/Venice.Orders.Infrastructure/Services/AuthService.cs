using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Venice.Orders.Domain.Services;

namespace Venice.Orders.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public sealed class AuthService(IConfiguration configuration): IAuthService
{
    public bool IsValidApiKey(string apiKey)
    {
        var apiKeyConfig = configuration["ApiKey"];
        return string.Compare(apiKey, apiKeyConfig, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public string GenerateToken(CancellationToken cancellationToken = default)
    {
        var jwtKey = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var expiresIn = int.Parse(configuration["Jwt:ExpiresInMinutes"] ?? "15");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(expiresIn),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}