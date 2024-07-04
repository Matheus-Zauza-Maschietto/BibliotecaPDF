using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace bibliotecaPDF.Services;

public class JsonWebTokensService
{
    private readonly IConfiguration _configuration;
    public JsonWebTokensService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(ClaimsIdentity ClaimsParaToken)
    {
        var informacoesParaToken = BuscarInformacoesDoToken();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = ClaimsParaToken,
            SigningCredentials = informacoesParaToken.SigningCredentials,
            Audience = informacoesParaToken.Audience,
            Issuer = informacoesParaToken.Issuer,
            Expires = informacoesParaToken.Expires
        };

        return GerarTokenString(tokenDescriptor);
    }

    private (SigningCredentials SigningCredentials, string Audience, string Issuer, DateTime Expires) BuscarInformacoesDoToken()
    {
        var signingCredentials = GerarCredenciais(_configuration["JwtBearerTokenSettings:SecretKey"] ?? "SecretKey not found");
        double lifeTimeInMinutes = double.Parse(_configuration["JwtBearerTokenSettings:ExpiryTimeInMinutes"] ?? "ExpiryTimeInMinutes key not found");
        string audience = _configuration["JwtBearerTokenSettings:Audience"] ?? "Audience not found";
        string issuer = _configuration["JwtBearerTokenSettings:Issuer"] ?? "Issuer not found";
        DateTime expires = DateTime.UtcNow.AddMinutes(lifeTimeInMinutes);

        return (signingCredentials, audience, issuer, expires);
    }

    private SigningCredentials GerarCredenciais(string secretKey)
    {
        byte[] key = Encoding.UTF8.GetBytes(secretKey);
        SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        return signingCredentials;
    }

    private string GerarTokenString(SecurityTokenDescriptor descriptor)
    {
        var TokenHandler = new JwtSecurityTokenHandler();
        var pretoken = TokenHandler.CreateToken(descriptor);
        return TokenHandler.WriteToken(pretoken);
    }

    public string GerarRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        token = CleanUpToken(token);
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["JwtBearerTokenSettings:Issuer"] ?? "Issuer not found",
            ValidAudience = _configuration["JwtBearerTokenSettings:Audience"] ?? "Audience not found",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtBearerTokenSettings:SecretKey"] ?? "SecretKey not found")
            )
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken securityToken;

        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                        out securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                  !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                 StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Token Invalido");

        return principal;
    }

    private string? CleanUpToken(string? token)
    {
        token = Regex.Replace(token, "bearer ", "", RegexOptions.IgnoreCase);
        return token;
    }
}
