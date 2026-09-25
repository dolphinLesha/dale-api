namespace Dale.Api.Domain;

public sealed class RefreshToken
{
    public Guid Guid { get; set; }

    public Guid UserGuid { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? Used { get; set; }

    public DateTimeOffset? Revoked { get; set; }

    public User User { get; set; } = null!;
}