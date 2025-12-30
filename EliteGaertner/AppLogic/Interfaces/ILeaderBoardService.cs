using Contracts.Enumeration;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

public interface ILeaderBoardService
{
    //Gibt das LeaderBoardDto an den Präsentations Layer zurück
    public LeaderboardDto GetLeaderBoardDto();
}