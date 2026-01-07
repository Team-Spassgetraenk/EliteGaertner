namespace PresentationLayer.State;

public class UserRegistrationState
{
    //UserID
    public int ProfileId { get; set; } = 0;

    // Kontaktdaten
    public string Email { get; set; } = "";
    public string Telephone { get; set; } = "";
    public bool ShareEmail { get; set; }
    public bool ShareTelephone { get; set; }

    // Ausgewählte Tags
    public HashSet<string> SelectedTags { get; set; } = new();
    
}