using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IProfileDbs
{

    //Gibt ein ProfilDto zurück
    public ProfileDto GetProfile(int profileId);

}