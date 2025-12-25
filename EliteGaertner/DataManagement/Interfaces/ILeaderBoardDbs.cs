using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace DataManagement.Interfaces;

public interface ILeaderBoardDbs
{
    //Prüft erstmal die Parameter auf Gültigkeit und entscheidet
    //welche Methoden die passenden LeaderboardEntryDtos zurückgeben
    public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int? tagId, LeaderboardSearchGoal goal);

    //Gibt die LeaderboardEntryDtos der User mit den meisten Likes zurück
    public IEnumerable<LeaderboardEntryDto> GetMostLikes();
    
    //Gibt die LeaderboardEntryDtos der User mit den längsten, breitesten oder schwersten Ernten zurück
    public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal);
}