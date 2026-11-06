using Dale.Api.Data;
using Dale.Api.Domain;
using Dale.Api.Repositories.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace Dale.Api.Infrastructure.PostgreSql;

public sealed class PsqlRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DaleDbContext _dbContext;

    public PsqlRefreshTokenRepository(DaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool TryGetByHash(string tokenHash, out RefreshToken token)
    {
        token = _dbContext.RefreshTokens
            .AsNoTracking()
            .Include(item => item.User)
            .FirstOrDefault(item => item.TokenHash == tokenHash)!;

        return token != null;
    }

    public void Add(RefreshToken token)
    {
        _dbContext.RefreshTokens.Add(token);
    }

    public bool TryMarkUsed(Guid tokenGuid, DateTimeOffset now)
    {
        var rowsChanged = _dbContext.RefreshTokens
            .Where(item => item.Guid == tokenGuid
                           && item.Used == null
                           && item.Revoked == null
                           && item.Expires > now)
            .ExecuteUpdate(setters => setters
                .SetProperty(item => item.Used, (DateTimeOffset?)now));

        return rowsChanged == 1;
    }
}