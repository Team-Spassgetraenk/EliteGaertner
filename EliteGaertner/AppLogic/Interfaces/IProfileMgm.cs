using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//Dieses Interface ist für alle Methoden zuständig, die sich um das Profilmanagement kümmern
public interface IProfileMgm
{
    //Überprüft, ob der Profilname existiert
    public bool CheckProfileNameExists (string profileName);
    
    //Gibt ein PublicProfileDto anhand der aufgerufenen ProfileId zurück
    public PublicProfileDto VisitPublicProfile(int profileId); //Für Besucher

    //Gibt ein PrivatesProfileDto anhand der aufgerufenen ProfileId zurück
    public PrivateProfileDto GetPrivProfile(int profileId); 

    //Änderungen am eigenen Profil werden hier verarbeitet 
    public void UpdateProfile(PrivateProfileDto profile);

    //Implementiert die Passwortänderung
    public void UpdateCredentials(CredentialProfileDto credentials);
    
    //Implementiert die Profilregistrierung
    public int RegisterProfile(PrivateProfileDto newProfile, CredentialProfileDto credentials);

    //Implementiert das Login
    public PrivateProfileDto LoginProfile(CredentialProfileDto credentials);
    
    //Die Präferenzen des Users werden hier abgearbeitet 
    public void SetPreference(List<PreferenceDto> preferences);
}