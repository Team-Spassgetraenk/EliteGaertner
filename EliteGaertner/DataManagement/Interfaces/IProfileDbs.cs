using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;

namespace DataManagement.Interfaces;

public interface IProfileDbs
{
    //Die Methode speichert ein neues Profil bei der Registrierung ab.
    //WICHTIG: Hier gibt die Methode eine DTO zurück, in der die HarvestUpload und PreferenceDtos
    //null sind! In der AppLogic muss man dann zu der PrivateProfileDto die HarvestUploads und PreferenceTags
    //mit einem neuinitialisierten PrivateProfileDto hinzufügen, die man dann in den Presentation Layer übergibt.
    public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials);

    //TODO Nicolas
    public PrivateProfileDto EditProfile(PrivateProfileDto privateProfile);
    
    //Gibt die Entität des Profils zurück
    public Profile? GetProfile(int profileId);
    
    //Erstellt aus der Entität Profil ein PrivateProfileDto
    public PrivateProfileDto GetPrivateProfile(int profileId);
    
    //Erstellt aus der Entität Profil ein PublicProfileDto
    public PublicProfileDto GetPublicProfile(int profileId);
    

    //TODO Nicolas
    public int? CheckPassword( string eMail, string passwordHash);
}