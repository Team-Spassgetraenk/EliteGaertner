using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace DataManagement.Interfaces;

//Diese Interface stellt alle Datenbankzugriffe für die HarvestUploads bereit
public interface IHarvestDbs
{

    //Gib mir die Harvestuploads des Profils zurück
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId);
    
    //Erstell mir eine Repository an HarvestUploadDtos die zum dem Interessensprofil
    //des Users passen. Die Menge der DTOs wird vom preloadCount bestimmt.
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds,
        HashSet<int> alreadyRatedProfiles, int preloadCount);
    
    //Erstellt ein HarvestUpload
    public void CreateUploadDbs(HarvestUploadDto uploadDto);
    
    //Gibt HarvestUploadDto zurück
    public HarvestUploadDto GetHarvestUploadDto(int uploadId);
    
    //Löscht einen HarvestUpload
    public void DeleteHarvestUpload(int uploadId);
    
    //Speichert einen Report der jeweiligen HarvestUpload ab
    public void SetReportHarvestUpload(int uploadId, ReportReasons reason);
    
    //Gibt zurück wie oft ein HarvestUpload Reported worden ist
    public int GetReportCount(int uploadId);
}