
namespace Contracts.Data_Transfer_Objects;

//Beinhaltet alle Informationen einer Profilbewertung
public record RateDto
{
    //Wie hat der Receiver den Creator bewertet?
    public int ContentReceiver { get; init; }
    public int ContentCreator { get; init; }
    public bool ContentReceiverValue { get; init; }
    //Wann hat der Creator den Receiver bewertet
    public DateTime ContentReceiverRatingDate { get; init; }
   

}