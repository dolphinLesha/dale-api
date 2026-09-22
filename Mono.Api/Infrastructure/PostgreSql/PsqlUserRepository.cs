using Microsoft.EntityFrameworkCore;
using Mono.Api.Data;
using Mono.Api.Domain;
using Mono.Api.Repositories.Users;

namespace Mono.Api.Infrastructure.PostgreSql;

public sealed class PsqlUserRepository : IUserRepository
{
    private readonly MonoDbContext _dbContext;

    public PsqlUserRepository(MonoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool TryGetByEmail(string email, out User user)
    {
        user = _dbContext.Users.FirstOrDefault(item => item.Email == email)!;

        return user != null;
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}