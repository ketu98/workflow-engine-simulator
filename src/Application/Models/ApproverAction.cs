namespace WorkflowEngineSimulator.Models;

public class ApproverAction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ApproverEmail { get; set; } = string.Empty;
    public ApprovalDecision? Decision { get; set; }
    public DateTime? ActionedOnUtc { get; set; }
    public string? Comments { get; set; }
}
