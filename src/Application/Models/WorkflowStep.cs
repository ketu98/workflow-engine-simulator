namespace WorkflowEngineSimulator.Models;

public class WorkflowStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsParallel { get; set; }
    public StepStatus Status { get; set; } = StepStatus.Pending;
    public List<ApproverAction> Approvers { get; set; } = new();
}
