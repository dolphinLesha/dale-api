using Dale.Api.Contracts.Auth;

namespace Dale.Api.Handlers.Auth;

public sealed class RegisterResult
{
    public bool IsSuccess { get; set; }

    public bool IsEmailAlreadyUsed { get; set; }

    public RegisterResponse Response { get; set; } = new();
}