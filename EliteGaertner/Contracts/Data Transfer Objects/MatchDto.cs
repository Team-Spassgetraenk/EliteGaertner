using System.Runtime.InteropServices.JavaScript;

namespace Contracts.Data_Transfer_Objects;

public record MatchDto
{
    public int MatchId { get; init; }
    public ProfileDto ContentReceiver { get; init; }
    public bool ContentReceiverValue { get; init; }
    public ProfileDto TargetProfile { get; init; }
    public bool TargetProfileValue { get; init; }
    public DateTime RatingDate { get; init; }
    public bool MatchActive { get; init; }
    public DateTime MatchActiveDate { get; init; }
    
}