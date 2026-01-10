namespace Contracts.Data_Transfer_Objects;

//Beinhaltet alle benötigten Informationen der Präferenz
public record PreferenceDto
{
    public int TagId { get; set; }
    public int Profileid { get; set; }
    public DateTime DateUpdated { get; set; }
}