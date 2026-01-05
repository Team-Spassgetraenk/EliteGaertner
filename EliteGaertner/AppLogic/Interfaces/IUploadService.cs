using Contracts.Data_Transfer_Objects;
using Microsoft.AspNetCore.Components.Forms;

namespace AppLogic.Interfaces;

//TODO Kommentare fehlen
public interface IUploadService
{
    public void CreateHarvestUpload(HarvestUploadDto uploadDto);
    public HarvestUploadDto GetUploadDto(int uploadId);
    public string? DeleteUpload(int uploadId); 
}