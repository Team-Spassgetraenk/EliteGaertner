using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;

namespace DataManagement.Interfaces;

//Dieses Interface stellt alle Profil-Datenbankzugriffe zur Verfügung
public interface IProfileDbs
{
  
    //Gibt zurück ob Profilname existiert 
    public bool CheckProfileNameExists(string profileName);
    
    //Erstellt ein neues Profil in der Datenbank 
    public int SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials);

    //Ändert Attribute eines Profils
    public void EditProfile(PrivateProfileDto privateProfile);

    //Ändert das Passwort eines Profils
    public void EditPassword(CredentialProfileDto credentials);
    
    //Gibt eine Profile-Entity zurück. Benötigen andere Methoden der Klasse
    public Profile? GetProfile(int profileId);
    
    //Gibt das passende PrivateProfileDto zurück
    public PrivateProfileDto GetPrivateProfile(int profileId);
    
    //Gibt das passende PublicProfileDto zurück 
    public PublicProfileDto GetPublicProfile(int profileId);
    
    //Überprüft, ob die Kombination aus Email und Passwort existiert und gibt das passende ProfileId zurück 
    public int? CheckPassword( string eMail, string passwordHash);
    
    //Gibt die passenden Profilpräferenzen zurück
    public IEnumerable<PreferenceDto> GetProfilePreference(int profileId);
    
    //Speichert die passenden Profilpräferenzen 
    public void SetProfilePreference(List<PreferenceDto> preferences);
}