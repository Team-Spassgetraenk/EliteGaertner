using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;

namespace AppLogic.Services;


public class LeaderboardService : ILeaderBoardService
{
    private readonly ILeaderBoardDbs _leaderBoardDbs;
    private readonly string _leaderboardTitle;
    private readonly int? _tagId;
    private readonly LeaderboardSearchGoal _goal;
    private readonly IReadOnlyList<LeaderboardEntryDto> _entries;
    
    public LeaderboardService(string leaderboardTitle, int? tagId, LeaderboardSearchGoal goal, ILeaderBoardDbs leaderBoardDbs)
    {
        _leaderBoardDbs = leaderBoardDbs;
        _leaderboardTitle = leaderboardTitle;
        _tagId = tagId;
        _goal = goal;
        _entries = _leaderBoardDbs.GetLeaderBoardEntries(_tagId, _goal).ToList();
    }

    public LeaderboardDto GetLeaderBoardDto()
        => new LeaderboardDto()
        {
            LeaderboardTitle = _leaderboardTitle,
            TagId = _tagId,
            Goal = _goal,
            Entries = _entries
        };
}