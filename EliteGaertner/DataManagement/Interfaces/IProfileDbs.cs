using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;

namespace DataManagement.Interfaces;

//TODO KOMMENTARE FEHLT
public interface IProfileDbs
{
    //Die Methode speichert ein neues Profil bei der Registrierung ab.
    //WICHTIG: Hier gibt die Methode eine DTO zurück, in der die HarvestUpload und PreferenceDtos
    //null sind! In der AppLogic muss man dann zu der PrivateProfileDto die HarvestUploads und PreferenceTags
    //mit einem neuinitialisierten PrivateProfileDto hinzufügen, die man dann in den Presentation Layer übergibt.
    
    public bool CheckUsernameExists(string username);
    
    public int SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials);

    public void EditProfile(PrivateProfileDto privateProfile);

    public void EditPassword(CredentialProfileDto credentials);
    
    public Profile? GetProfile(int profileId);
    
    public PrivateProfileDto GetPrivateProfile(int profileId);
    
    public PublicProfileDto GetPublicProfile(int profileId);
    
    public int? CheckPassword( string eMail, string passwordHash);
    
    public IEnumerable<PreferenceDto> GetUserPreference(int profileId);
    
    public void SetUserPreference(List<PreferenceDto> preferences);
}