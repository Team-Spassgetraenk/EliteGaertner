using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IPreferenceDbs
{
    
    //Gib die Preference des Users zurück
    public IEnumerable<PreferenceDto> GetUserPreference(int profileId);

    
    //Änder die Preference des Users
    public void SetUserPreference(int profileId, PreferenceDto newUserPreference);

}