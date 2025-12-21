using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;

namespace DataManagement.Interfaces;

public interface IProfileDbs
{
    //Gibt die Entität des Profils zurück
    public Profile? GetProfile(int profileId);
    
    //Erstellt aus der Entität Profil ein PrivateProfileDto
    public PrivateProfileDto GetPrivateProfile(int profileId);
    
    //Erstellt aus der Entität Profil ein PublicProfileDto
    public PublicProfileDto GetPublicProfile(int profileId);
    
    

}