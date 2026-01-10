namespace Contracts.Data_Transfer_Objects;

//Stellt Email und Passwort des Profils bereit
public record CredentialProfileDto
{
    public string EMail { get; init; }
    public string PasswordHash { get; init; }
}