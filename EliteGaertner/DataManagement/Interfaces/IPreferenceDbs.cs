using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IPreferenceDbs
{
    
    //Gib die Preference des Users zurück
    public PreferenceDto GetUserPreference(int userId);

    
    //Änder die Preference des Users
    public void SetUserPreference(int userId, PreferenceDto newUserPreference);

}