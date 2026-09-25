using Dale.Api.Contracts.Auth;

namespace Dale.Api.Handlers.Auth;

public interface IAuthHandler
{
    RegisterResult HandleRegister(RegisterRequest request);
}