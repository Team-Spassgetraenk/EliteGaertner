namespace PresentationLayer.State;

public class UserRegistrationState
{
    // Kontaktdaten
    public string Email { get; set; } = "";
    public string Telephone { get; set; } = "";
    public bool ShareEmail { get; set; }
    public bool ShareTelephone { get; set; }

    // Ausgewählte Tags
    public HashSet<string> SelectedTags { get; set; } = new();
    
}