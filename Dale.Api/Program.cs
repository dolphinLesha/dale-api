using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dale.Api.Data;
using Dale.Api.Domain;
using Dale.Api.Handlers.Auth;
using Dale.Api.Infrastructure.PostgreSql;
using Dale.Api.Repositories.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<DaleDbContext>(options =>
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