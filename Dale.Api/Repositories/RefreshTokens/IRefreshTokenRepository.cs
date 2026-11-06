using Dale.Api.Domain;

namespace Dale.Api.Repositories.RefreshTokens;

public interface IRefreshTokenRepository
{
    bool TryGetByHash(string tokenHash, out RefreshToken token);
    
    void Add(RefreshToken token);

    bool TryMarkUsed(Guid tokenGuid, DateTimeOffset now);
}