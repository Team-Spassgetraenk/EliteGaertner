namespace DataManagement.Entities;

public class Profile
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EMail { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public string profileText { get; set; }
    public bool ShareMail { get; set; }
    public bool SharePhoneNumber { get; set; }
    public DateTime UserCreated { get; set; }
    public string ProfilePicture { get; set; }
}