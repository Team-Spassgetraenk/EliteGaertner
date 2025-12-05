namespace Contracts.Data_Transfer_Objects;

public record MatchDto
{
    public ProfileDto ContentReceiver { get; init; }
    public bool ContentReceiverValue { get; init; }
    public ProfileDto TargetUser { get; init; }
    public bool TargetUserValue { get; init; }
}