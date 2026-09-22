using Mono.Api.Domain;

namespace Mono.Api.Repositories.Users;

public interface IUserRepository
{
    bool TryGetByEmail(string email, out User user);

    void Add(User user);
}