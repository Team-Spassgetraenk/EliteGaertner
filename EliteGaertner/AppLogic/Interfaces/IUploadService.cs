using Contracts.Data_Transfer_Objects;
using Microsoft.AspNetCore.Components.Forms;

namespace AppLogic.Interfaces;

public interface IUploadService
{
    
    
    bool CreateUpload(int userId, string imageUrl, string description, float weight, int quantity, DateTime uploadDate);

    bool CreateUpload(HarvestUploadDto uploadDto);
    
    bool DeleteUpload(int uploadId, int userId);

    List<HarvestUploadDto> GetUserUploads(int userId);
}