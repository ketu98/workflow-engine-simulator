namespace WorkflowEngineSimulator.Models;

public class WorkflowRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string RequestorEmail { get; set; } = string.Empty;
    public WorkflowStatus Status { get; set; } = WorkflowStatus.Pending;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public List<WorkflowStep> Steps { get; set; } = new();
}
