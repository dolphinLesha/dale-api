using Dale.Api.Contracts.Auth;
using Dale.Api.Domain;
using Dale.Api.Repositories.Users;
using Microsoft.AspNetCore.Identity;

namespace Dale.Api.Handlers.Auth;

public sealed class AuthHandler : IAuthHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public RegisterResult HandleRegister(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var result = new RegisterResult();

        if (_userRepository.TryGetByEmail(normalizedEmail, out _))
        {
            result.IsEmailAlreadyUsed = true;
        }
        else
        {
            var user = new User
            {
                Guid = Guid.NewGuid(),
                Email = normalizedEmail,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Created = DateTimeOffset.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _userRepository.Add(user);

            result.IsSuccess = true;
            result.Response = new RegisterResponse
            {
                Guid = user.Guid,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created
            };
        }

        return result;
    }
}