using Dale.Api.Contracts.Auth;
using Dale.Api.Data;
using Dale.Api.Domain;
using Dale.Api.Options;
using Dale.Api.Repositories.RefreshTokens;
using Dale.Api.Repositories.Users;
using Dale.Api.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Dale.Api.Handlers.Auth;

public sealed class AuthHandler : IAuthHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly DaleDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public AuthHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService,
        DaleDbContext dbContext,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    public AuthResult HandleRegister(RegisterRequest request)
    {
        var result = new AuthResult();

        if (string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password)
            || string.IsNullOrWhiteSpace(request.FirstName)
            || string.IsNullOrWhiteSpace(request.LastName))
        {
            result.Status = AuthStatus.InvalidRequest;
        }
        else
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (_userRepository.TryGetByEmail(email, out _))
            {
                result.Status = AuthStatus.EmailAlreadyUsed;
            }
            else
            {
                var user = new User
                {
                    Guid = Guid.NewGuid(),
                    Email = email,
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    Created = DateTimeOffset.UtcNow
                };

                user.PasswordHash = _passwordHasher.HashPassword(
                    user, request.Password);

                _userRepository.Add(user);

                var tokens = CreateAndStageTokens(user);

                _dbContext.SaveChanges();

                result.Status = AuthStatus.Success;
                result.Tokens = tokens;
            }
        }

        return result;
    }

    public AuthResult HandleLogin(LoginRequest request)
    {
        var result = new AuthResult();

        if (string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password))
        {
            result.Status = AuthStatus.InvalidRequest;
        }
        else
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (_userRepository.TryGetByEmail(email, out var user)
                && user.Deleted == null
                && _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password) != PasswordVerificationResult.Failed)
            {
                var tokens = CreateAndStageTokens(user);

                _dbContext.SaveChanges();

                result.Status = AuthStatus.Success;
                result.Tokens = tokens;
            }
            else
            {
                result.Status = AuthStatus.InvalidCredentials;
            }
        }

        return result;
    }

    public AuthResult HandleRefresh(RefreshRequest request)
    {
        var result = new AuthResult
        {
            Status = AuthStatus.InvalidRefreshToken
        };

        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var tokenHash = _tokenService.HashRefreshToken(
                request.RefreshToken);

            if (_refreshTokenRepository.TryGetByHash(tokenHash, out var storedToken)
                && storedToken.User != null
                && storedToken.User.Deleted == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                if (_refreshTokenRepository.TryMarkUsed(
                    storedToken.Guid,
                    DateTimeOffset.UtcNow))
                {
                    var tokens = CreateAndStageTokens(storedToken.User);

                    _dbContext.SaveChanges();
                    transaction.Commit();

                    result.Status = AuthStatus.Success;
                    result.Tokens = tokens;
                }
            }
        }

        return result;
    }

    private AuthTokensResponse CreateAndStageTokens(User user)
    {
        var now = DateTimeOffset.UtcNow;
        var accessExpires = now.AddMinutes(
            _jwtOptions.AccessTokenLifetimeMinutes);
        var refreshExpires = now.AddHours(
            _jwtOptions.RefreshTokenLifetimeHours);

        var refreshToken = _tokenService.CreateRefreshToken();

        _refreshTokenRepository.Add(new RefreshToken
        {
            Guid = Guid.NewGuid(),
            UserGuid = user.Guid,
            TokenHash = _tokenService.HashRefreshToken(refreshToken),
            Created = now,
            Expires = refreshExpires
        });

        var response = new AuthTokensResponse
        {
            AccessToken = _tokenService.CreateAccessToken(user, accessExpires),
            AccessTokenExpires = accessExpires,
            RefreshToken = refreshToken,
            RefreshTokenExpires = refreshExpires
        };

        return response;
    }
}