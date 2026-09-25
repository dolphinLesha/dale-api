namespace Dale.Api.Contracts.Auth;

public sealed class RegisterResponse
{
    public Guid Guid { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTimeOffset Created { get; set; }
}
