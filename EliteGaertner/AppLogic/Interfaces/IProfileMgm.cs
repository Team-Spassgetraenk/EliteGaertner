using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//TODO Kommentare fehlen
public interface IProfileMgm
{
    public bool CheckUsernameExists (string username);
    
    public PublicProfileDto VisitPublicProfile(int profileId); //Für Besucher

    public PrivateProfileDto GetPrivProfile(int profileId); //Für das eigene

    public void UpdateProfile(PrivateProfileDto profile);

    public void UpdateCredentials(CredentialProfileDto credentials);
    
    public int RegisterProfile(PrivateProfileDto newProfile, CredentialProfileDto credentials);

    public PrivateProfileDto LoginProfile(CredentialProfileDto credentials);
    
    
    public void SetPreference(List<PreferenceDto> preferences);
}