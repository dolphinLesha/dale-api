using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mono.Api.Data;
using Mono.Api.Domain;
using Mono.Api.Handlers.Auth;
using Mono.Api.Infrastructure.PostgreSql;
using Mono.Api.Repositories.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<MonoDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IUserRepository, PsqlUserRepository>();

builder.Services.AddScoped<IAuthHandler, AuthHandler>();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();