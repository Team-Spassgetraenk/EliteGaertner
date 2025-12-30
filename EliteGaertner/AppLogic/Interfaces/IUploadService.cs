using Contracts.Data_Transfer_Objects;
using Microsoft.AspNetCore.Components.Forms;

namespace AppLogic.Interfaces;

public interface IUploadService
{
   bool CreateHarvestUpload(int profileId, string imageUrl, string description, float weight, int width, int length);

   bool CreateHarvestUpload(HarvestUploadDto uploadDto);
    
    string DeleteUpload(int uploadId, int profileId);

    List<HarvestUploadDto> GetUserUploads(int profileId);
}