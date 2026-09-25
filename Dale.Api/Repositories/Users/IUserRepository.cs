using Dale.Api.Domain;

namespace Dale.Api.Repositories.Users;

public interface IUserRepository
{
    bool TryGetByEmail(string email, out User user);

    void Add(User user);
}