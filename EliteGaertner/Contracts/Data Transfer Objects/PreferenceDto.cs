namespace Contracts.Data_Transfer_Objects;

public record PreferenceDto
{
    public int TagId { get; set; }
    public int Profileid { get; set; }
    public DateTime DateUpdated { get; set; }
}