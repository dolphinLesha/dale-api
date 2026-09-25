namespace Dale.Api.Contracts.Users;

public sealed class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } =  string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateOnly? Birthday { get; set; }
    public string Description { get; set; } = string.Empty;
}