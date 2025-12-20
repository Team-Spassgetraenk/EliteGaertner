namespace Contracts.Data_Transfer_Objects;

public record ContactVisibilityDto
{
    public int UserId { get; init; }
    public string EMail { get; init; }
     public string PhoneNumber { get; init; }
    public bool ShareMail { get; init; }
    public bool SharePhoneNumber { get; init; }
};