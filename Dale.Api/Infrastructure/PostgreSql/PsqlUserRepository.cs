using Dale.Api.Data;
using Dale.Api.Domain;
using Dale.Api.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Dale.Api.Infrastructure.PostgreSql;

public sealed class PsqlUserRepository : IUserRepository
{
    private readonly DaleDbContext _dbContext;

    public PsqlUserRepository(DaleDbContext dbContext)
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