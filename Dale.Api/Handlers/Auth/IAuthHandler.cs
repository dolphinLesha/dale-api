using Dale.Api.Contracts.Auth;

namespace Dale.Api.Handlers.Auth;

public interface IAuthHandler
{
    AuthResult HandleRegister(RegisterRequest request);

    AuthResult HandleLogin(LoginRequest request);

    AuthResult HandleRefresh(RefreshRequest request);
}