using AppLogic.Interfaces;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using Microsoft.Extensions.Logging;

namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    private readonly IHarvestDbs _harvestDbs;
    private readonly ILogger<UploadServiceImpl> _logger;
    
    public UploadServiceImpl(IHarvestDbs harvestDbs, ILogger<UploadServiceImpl> logger)
    {
        _harvestDbs = harvestDbs;
        _logger = logger;
    }
    
    public void CreateHarvestUpload(HarvestUploadDto uploadDto)
    {
        try
        {
            _logger.LogInformation("Creating harvest upload for ProfileId: {ProfileId}, ImageUrl: {ImageUrl}", uploadDto.ProfileId, uploadDto.ImageUrl);
            _harvestDbs.CreateUploadDbs(uploadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating harvest upload");
            throw new InvalidOperationException("Upload fehlgeschlagen", ex);
        }
    }

    public string? DeleteHarvestUpload(int uploadId) // gibt die ImageUrl zurück, damit die UI die Datei in wwwroot löschen kann
    {
        _logger.LogInformation("DeleteHarvestUpload called with uploadId: {UploadId}", uploadId);
        HarvestUploadDto uploadDto;
        
        //Lädt HarvestUpload aus Datenbank
        try
        {
            uploadDto = _harvestDbs.GetHarvestUploadDto(uploadId);
            _logger.LogDebug("Loaded HarvestUploadDto for uploadId: {UploadId}, ImageUrl: {ImageUrl}", uploadId, uploadDto?.ImageUrl);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Upload konnte nicht geladen werden.", ex);
        }
        
        if (uploadDto == null || string.IsNullOrWhiteSpace(uploadDto.ImageUrl))
        {
            _logger.LogWarning("HarvestUploadDto is null or ImageUrl is empty for uploadId: {UploadId}", uploadId);
            return null;
        }

        //Speichert ImageUrl ab
        var imageUrl = uploadDto.ImageUrl;
        
        //Löscht HarvestUpload in der Datenbank
        try
        {
            _logger.LogInformation("Deleting harvest upload with uploadId: {UploadId}", uploadId);
            _harvestDbs.DeleteHarvestUpload(uploadId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting harvest upload with uploadId: {UploadId}", uploadId);
            throw new InvalidOperationException("Upload konnte nicht gelöscht werden.", ex);
        }
        
        //Gibt ImageUrl zurück
        return imageUrl;
    }
}