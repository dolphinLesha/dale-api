namespace Dale.Api.Contracts.Auth;

public sealed class AuthTokensResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTimeOffset AccessTokenExpires { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public DateTimeOffset RefreshTokenExpires { get; set; }
}