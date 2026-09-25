namespace Dale.Api.Domain;

public sealed class User
{
    public Guid Guid { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public DateOnly? Birthday { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public DateTimeOffset? Deleted { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}