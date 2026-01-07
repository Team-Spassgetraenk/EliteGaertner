using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//TODO Kommentare fehlen
public interface IProfileMgm
{
    public bool CheckUsernameExists (string username);
    
    public PublicProfileDto VisitPublicProfile(int profileId); //Für Besucher

    public PrivateProfileDto GetPrivProfile(int profileId); //Für das eigene

    public bool UpdateProfile(PrivateProfileDto profile);

    public void UpdateCredentials(CredentialProfileDto credentials);

    public bool UpdateContactVisibility(ContactVisibilityDto dto);
    
    public PrivateProfileDto RegisterProfile(PrivateProfileDto newProfile, CredentialProfileDto credentials);

    public PrivateProfileDto LoginProfile(CredentialProfileDto credentials);

    public List<PreferenceDto> GetPreference(int profileId);
    
    public bool SetPreference(List<PreferenceDto> preferences);
}