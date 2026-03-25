using WorkflowEngineSimulator.Models;

namespace WorkflowEngineSimulator.Services;

public class WorkflowService
{
    private static readonly List<WorkflowRequest> Requests = new();

    public WorkflowRequest Create(string title, string requestorEmail)
    {
        var request = new WorkflowRequest
        {
            Title = title,
            RequestorEmail = requestorEmail,
            Status = WorkflowStatus.InProgress,
            Steps = new List<WorkflowStep>
            {
                new()
                {
                    Order = 1,
                    Name = "Manager Approval",
                    IsParallel = false,
                    Status = StepStatus.InProgress,
                    Approvers = new List<ApproverAction>
                    {
                        new() { ApproverEmail = "manager@company.com" }
                    }
                },
                new()
                {
                    Order = 2,
                    Name = "Finance & Legal Approval",
                    IsParallel = true,
                    Status = StepStatus.Pending,
                    Approvers = new List<ApproverAction>
                    {
                        new() { ApproverEmail = "finance@company.com" },
                        new() { ApproverEmail = "legal@company.com" }
                    }
                }
            }
        };

        Requests.Add(request);
        return request;
    }

    public List<WorkflowRequest> GetAll() => Requests;

    public WorkflowRequest? GetById(Guid id) =>
        Requests.FirstOrDefault(x => x.Id == id);

    public bool TakeAction(Guid requestId, Guid stepId, string approverEmail, ApprovalDecision decision, string? comments)
    {
        var request = GetById(requestId);
        if (request == null) return false;

        if (request.Status is WorkflowStatus.Approved or WorkflowStatus.Rejected)
            return false;

        var step = request.Steps.FirstOrDefault(x => x.Id == stepId);
        if (step == null) return false;

        var approver = step.Approvers.FirstOrDefault(x =>
            x.ApproverEmail.Equals(approverEmail, StringComparison.OrdinalIgnoreCase));

        if (approver == null) return false;

        if (approver.Decision.HasValue) return false;

        approver.Decision = decision;
        approver.ActionedOnUtc = DateTime.UtcNow;
        approver.Comments = comments;

        if (decision == ApprovalDecision.Rejected)
        {
            step.Status = StepStatus.Rejected;
            request.Status = WorkflowStatus.Rejected;
            return true;
        }

        if (step.IsParallel)
        {
            if (step.Approvers.All(x => x.Decision == ApprovalDecision.Approved))
            {
                step.Status = StepStatus.Approved;
                MoveToNextStep(request, step.Order);
            }
        }
        else
        {
            step.Status = StepStatus.Approved;
            MoveToNextStep(request, step.Order);
        }

        return true;
    }

    private static void MoveToNextStep(WorkflowRequest request, int currentOrder)
    {
        var nextStep = request.Steps
            .Where(x => x.Order > currentOrder)
            .OrderBy(x => x.Order)
            .FirstOrDefault();

        if (nextStep == null)
        {
            request.Status = WorkflowStatus.Approved;
            return;
        }

        nextStep.Status = StepStatus.InProgress;
    }
}
