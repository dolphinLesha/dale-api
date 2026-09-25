using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dale.Api.Domain;
using Dale.Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dale.Api.Services.Auth;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SigningKey));

        _signingCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);
    }

    public string CreateAccessToken(User user, DateTimeOffset expires)
    {
        var now = DateTimeOffset.UtcNow;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: _signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(bytes);

        return refreshToken;
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);
        var tokenHash = Convert.ToHexString(hash);

        return tokenHash;
    }
}