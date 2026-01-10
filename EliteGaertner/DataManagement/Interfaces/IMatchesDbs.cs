using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

//Dieses Interface stellt alle Datenbankzugriffe zur Verfügung, die für die MatchLogik benötigt wird
public interface IMatchesDbs
{
    //Gibt die aktiven Matches des ContentReceivers zurück
    public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver);

    //Speichert Bewertungen ab
    public void SaveRateInfo(RateDto matchDto);

    //Gibt die bereits bewerteten Profile des ContentReceivers zurück
    public HashSet<int> GetAlreadyRatedProfileIds(int profileIdReceiver);
}