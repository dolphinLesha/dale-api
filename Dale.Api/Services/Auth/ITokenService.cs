using Dale.Api.Domain;

namespace Dale.Api.Services.Auth;

public interface ITokenService
{
    string CreateAccessToken(User user, DateTimeOffset expires);

    string CreateRefreshToken();

    string HashRefreshToken(string refreshToken);
}