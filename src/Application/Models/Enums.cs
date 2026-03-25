namespace WorkflowEngineSimulator.Models;

public enum WorkflowStatus
{
    Pending,
    InProgress,
    Approved,
    Rejected
}

public enum StepStatus
{
    Pending,
    InProgress,
    Approved,
    Rejected
}

public enum ApprovalDecision
{
    Approved,
    Rejected
}
