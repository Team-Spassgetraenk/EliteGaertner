using Contracts.Enumeration;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//Dieses Interface stellt alle Methoden zur Verfügung, die für die Erstellung einer 
//Rangliste benötigt werden.
public interface ILeaderBoardService
{
    //Gibt das LeaderBoardDto an den Präsentations Layer zurück
    public LeaderboardDto GetLeaderBoardDto();
}