using Contracts.Data_Transfer_Objects;
using Microsoft.AspNetCore.Components.Forms;

namespace AppLogic.Interfaces;

//Diese Interfaces stellt alle Methoden zur Verfügung, die den HarvestUpload behandeln
public interface IUploadService
{
    //Erstellt HarvestUpload
    public void CreateHarvestUpload(HarvestUploadDto uploadDto);

    //Löscht HarvestUpload
    public string? DeleteHarvestUpload(int uploadId); 
}