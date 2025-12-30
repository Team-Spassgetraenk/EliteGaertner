namespace Contracts.Data_Transfer_Objects;

public record CredentialProfileDto
{
    public string EMail { get; init; }
    public string PasswordHash { get; init; }
}