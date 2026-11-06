using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dale.Api.Data;
using Dale.Api.Domain;
using Dale.Api.Handlers.Auth;
using Dale.Api.Infrastructure.PostgreSql;
using Dale.Api.Repositories.Users;
using Dale.Api.Options;
using Dale.Api.Services.Auth;
using Dale.Api.Repositories.RefreshTokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>() ?? throw new InvalidOperationException(
    "Jwt configuration is missing.");

if (string.IsNullOrWhiteSpace(jwtOptions.Issuer)
    || string.IsNullOrWhiteSpace(jwtOptions.Audience)
    || Encoding.UTF8.GetByteCount(jwtOptions.SigningKey) < 32
    || jwtOptions.AccessTokenLifetimeMinutes <= 0
    || jwtOptions.RefreshTokenLifetimeHours <= 0)
{
    throw new InvalidOperationException("Jwt configuration is invalid.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<ITokenService, TokenService>();

var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<DaleDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddScoped<IUserRepository, PsqlUserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, PsqlRefreshTokenRepository>();

builder.Services.AddScoped<IAuthHandler, AuthHandler>();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();