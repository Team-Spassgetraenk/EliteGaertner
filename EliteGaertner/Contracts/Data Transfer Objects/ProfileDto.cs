namespace Contracts.Data_Transfer_Objects;

public record ProfileDto
{
    public int UserId { get; init; }
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string EMail { get; init; }
    public string PasswordHash { get; init; }
    public string profileText { get; init; }
}