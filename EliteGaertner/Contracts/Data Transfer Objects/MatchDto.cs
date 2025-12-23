using System.Runtime.InteropServices.JavaScript;

namespace Contracts.Data_Transfer_Objects;

//Manche Properties hier sind nullable, da auch keine Ratings vorhanden sein können 
public record RateDto
{
    //Wie hat der Receiver den Creator bewertet?
    public int ContentReceiver { get; init; }
    public int ContentCreator { get; init; }
    public bool ContentReceiverValue { get; init; }
    //Wann hat der Creator den Receiver bewertet
    public DateTime ContentReceiverRatingDate { get; init; }
   

}