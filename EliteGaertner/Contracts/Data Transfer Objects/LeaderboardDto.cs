using Contracts.Enumeration;

namespace Contracts.Data_Transfer_Objects;

//TODO
public record LeaderboardDto
{
    public string LeaderboardTitle { get; init; }
    public int? TagId { get; init; }
    public LeaderboardSearchGoal Goal { get; init; }
    public IReadOnlyList<LeaderboardEntryDto> Entries { get; init; }
}