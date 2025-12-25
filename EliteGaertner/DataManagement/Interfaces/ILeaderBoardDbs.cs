using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace DataManagement.Interfaces;

public interface ILeaderBoardDbs
{

    //Gibt die persönliche Ranglistenplatzierung zurück
    public LeaderboardEntryDto GetPersonalLeaderBoardEntry(int profileId, int? tagId, LeaderboardSearchGoal goal);
    
    //Gibt die Top-Ranglistenplatzierungen zurück
    public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int profileId, int? tagId, LeaderboardSearchGoal goal);

    //Parameter überprüfen für persönliche Ranglistenplatzierung
    public bool CheckParametersForPersonal(int profileId, int? tagId, LeaderboardSearchGoal goal);
    
    //Parameter überprüfen für Top-Ranglistenplatzierungen
    public bool CheckParametersForTopList(int? tagId, LeaderboardSearchGoal goal);

    //Gibt die LeaderboardEntryDtos der User mit den meisten Likes zurück
    public IEnumerable<LeaderboardEntryDto> GetMostLikes();
    
    //Gibt die LeaderboardEntryDtos der User mit den längsten, breitesten oder schwersten Ernten zurück
    public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal);

    //Gibt die Platzierung des eingeloggten Profils zurück (Likes)
    public LeaderboardEntryDto GetPersonalLikes(int profileId);

    //Gibt die Platzierung des eingeloggten Profils zurück (Harvest)
    public LeaderboardEntryDto GetPersonalHarvestEntry(int profileId, int tagId, LeaderboardSearchGoal goal);
}