using System.Runtime.InteropServices.JavaScript;

namespace Contracts.Data_Transfer_Objects;

//Manche Properties hier sind nullable, da auch keine Ratings vorhanden sein können 
public record MatchDto
{
    //Wie hat der Receiver den Creator bewertet?
    public int? ContentReceiver { get; init; }
    public bool ContentReceiverValue { get; init; }
    
    //Wie hat der Creator den Receiver bewertet?
    public int? ContentCreator { get; init; }
    public bool ContentCreatorValue { get; init; }
    
    //Wann hat der Creator den Receiver bewertet, vice versa
    public DateTime? ContentReceiverRatingDate { get; init; }
    public DateTime? ContentCreatorRatingDate { get; init; }

}