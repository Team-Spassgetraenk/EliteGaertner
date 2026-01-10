namespace Contracts.Data_Transfer_Objects;


//Beinhaltet alle Informationen einer Ranglistenplatzierung
public record LeaderboardEntryDto
{
    public int Rank { get; init; }
    public int ProfileId { get; init; }
    public string UserName { get; init; }
    public float Value { get; init; }
}