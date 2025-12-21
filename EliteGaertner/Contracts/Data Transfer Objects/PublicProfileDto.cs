namespace Contracts.Data_Transfer_Objects;

public record PublicProfileDto
{
    public int ProfileId { get; init; }
    public string UserName { get; init; }
    public string EMail { get; init; }
    public string Phonenumber { get; init; }
    public string Profiletext { get; init; }
    public bool ShareMail { get; init; }
    public bool SharePhoneNumber { get; init; }
    public DateTime UserCreated { get; init; }
    public List <HarvestUploadDto> HarvestUploads { get; init; }
}