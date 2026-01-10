using AppLogic.Interfaces;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    private readonly IHarvestDbs _harvestDbs;
    
    public UploadServiceImpl(IHarvestDbs harvestDbs)
    {
        _harvestDbs = harvestDbs;
    }
    
    public void CreateHarvestUpload(HarvestUploadDto uploadDto)
    {
        try
        {
            _harvestDbs.CreateUploadDbs(uploadDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Upload fehlgeschlagen", ex);
        }
    }

    public string? DeleteHarvestUpload(int uploadId) // gibt die ImageUrl zurück, damit die UI die Datei in wwwroot löschen kann
    {
        HarvestUploadDto uploadDto;
        
        //Lädt HarvestUpload aus Datenbank
        try
        {
            uploadDto = _harvestDbs.GetHarvestUploadDto(uploadId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Upload konnte nicht geladen werden.", ex);
        }
        
        if (uploadDto == null || string.IsNullOrWhiteSpace(uploadDto.ImageUrl))
            return null;

        //Speichert ImageUrl ab
        var imageUrl = uploadDto.ImageUrl;
        
        //Löscht HarvestUpload in der Datenbank
        try
        {
            _harvestDbs.DeleteHarvestUpload(uploadId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Upload konnte nicht gelöscht werden.", ex);
        }
        
        //Gibt ImageUrl zurück
        return imageUrl;
    }
}