using Contracts.Data_Transfer_Objects;

namespace Infrastructure.Interfaces;

public interface IPreferenceDBS
{
    
    //Gib die Preference des Users zurück
    public PreferenceDto GetUserPreference(int userId);

    
    //Änder die Preference des Users
    public void SetUserPreference(int userId, PreferenceDto newUserPreference);

}