using Mono.Api.Contracts.Auth;

namespace Mono.Api.Handlers.Auth;

public interface IAuthHandler
{
    RegisterResult HandleRegister(RegisterRequest request);
}