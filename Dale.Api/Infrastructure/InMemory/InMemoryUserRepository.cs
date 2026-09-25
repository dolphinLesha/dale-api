using Dale.Api.Domain;
using Dale.Api.Repositories.Users;

namespace Dale.Api.Infrastructure.InMemory;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public bool TryGetByEmail(string email, out User user)
    {
        user = _users.FirstOrDefault(item => item.Email == email)!;

        return user != null;
    }

    public void Add(User user)
    {
        _users.Add(user);
    }
}