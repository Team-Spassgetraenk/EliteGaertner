namespace Contracts.Data_Transfer_Objects;

public record CredentialProfileDto
{
    public int ProfileId { get; init; }
    public string EMail { get; init; }
    public string PasswordHash { get; init; }
}