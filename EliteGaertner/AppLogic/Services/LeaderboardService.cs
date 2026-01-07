using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;

namespace AppLogic.Services;


public class LeaderboardService : ILeaderBoardService
{
    private readonly ILeaderBoardDbs _leaderBoardDbs;
    private readonly string _leaderboardTitle;
    private readonly int _leaderboardId;
    private readonly int _profileId;
    private readonly int? _tagId;
    private readonly LeaderboardSearchGoal _goal;
    private readonly LeaderboardEntryDto? _personalEntry;
    private readonly IReadOnlyList<LeaderboardEntryDto> _entries;
    
    public LeaderboardService(string leaderboardTitle, int leaderboardId, int profileId, int? tagId, LeaderboardSearchGoal goal, ILeaderBoardDbs leaderBoardDbs)
    {
        _leaderBoardDbs = leaderBoardDbs;
        _leaderboardTitle = leaderboardTitle;
        _leaderboardId = leaderboardId;
        _profileId = profileId;
        _tagId = tagId;
        _goal = goal;
        _personalEntry = _leaderBoardDbs.GetPersonalLeaderBoardEntry(_profileId, _tagId, _goal);
        _entries = _leaderBoardDbs.GetLeaderBoardEntries(_profileId, _tagId, _goal).ToList();
    }

    public LeaderboardDto GetLeaderBoardDto()
        => new LeaderboardDto()
        {
            LeaderboardTitle = _leaderboardTitle,
            LeaderboardId = _leaderboardId,
            TagId = _tagId,
            Goal = _goal,
            PersonalEntry = _personalEntry,
            Entries = _entries
        };
}