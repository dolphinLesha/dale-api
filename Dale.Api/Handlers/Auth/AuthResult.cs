using Dale.Api.Contracts.Auth;

namespace Dale.Api.Handlers.Auth;

public enum AuthStatus
{
    Success,
    InvalidRequest,
    EmailAlreadyUsed,
    InvalidCredentials,
    InvalidRefreshToken
}

public sealed class AuthResult
{
    public AuthStatus Status { get; set; }

    public AuthTokensResponse Tokens { get; set; } = new();
}