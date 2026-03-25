using FluentAssertions;
using WorkflowEngineSimulator.Models;
using WorkflowEngineSimulator.Services;
using Xunit;

namespace WorkflowEngineSimulator.Tests;

public class WorkflowServiceTests
{
    [Fact]
    public void Create_Should_Create_Workflow_With_Initial_InProgress_Status()
    {
        var service = new WorkflowService();

        var result = service.Create("Pricing Approval", "aniket@company.com");

        result.Should().NotBeNull();
        result.Title.Should().Be("Pricing Approval");
        result.RequestorEmail.Should().Be("aniket@company.com");
        result.Status.Should().Be(WorkflowStatus.InProgress);
        result.Steps.Should().HaveCount(2);
        result.Steps.First().Status.Should().Be(StepStatus.InProgress);
    }

    [Fact]
    public void TakeAction_Should_Reject_Workflow_When_Approver_Rejects()
    {
        var service = new WorkflowService();

        var workflow = service.Create("Pricing Approval", "aniket@company.com");
        var firstStep = workflow.Steps.First();

        var success = service.TakeAction(
            workflow.Id,
            firstStep.Id,
            "manager@company.com",
            ApprovalDecision.Rejected,
            "Rejected by manager");

        success.Should().BeTrue();

        workflow.Status.Should().Be(WorkflowStatus.Rejected);
        firstStep.Status.Should().Be(StepStatus.Rejected);
    }

    [Fact]
    public void TakeAction_Should_Move_To_Next_Step_When_Sequential_Step_Is_Approved()
    {
        var service = new WorkflowService();

        var workflow = service.Create("Pricing Approval", "aniket@company.com");
        var firstStep = workflow.Steps.First();
        var secondStep = workflow.Steps.Skip(1).First();

        var success = service.TakeAction(
            workflow.Id,
            firstStep.Id,
            "manager@company.com",
            ApprovalDecision.Approved,
            "Approved");

        success.Should().BeTrue();

        firstStep.Status.Should().Be(StepStatus.Approved);
        secondStep.Status.Should().Be(StepStatus.InProgress);
        workflow.Status.Should().Be(WorkflowStatus.InProgress);
    }

    [Fact]
    public void TakeAction_Should_Approve_Workflow_When_All_Parallel_Approvers_Approve()
    {
        var service = new WorkflowService();

        var workflow = service.Create("Pricing Approval", "aniket@company.com");
        var firstStep = workflow.Steps.First();
        var secondStep = workflow.Steps.Skip(1).First();

        service.TakeAction(
            workflow.Id,
            firstStep.Id,
            "manager@company.com",
            ApprovalDecision.Approved,
            "Approved");

        service.TakeAction(
            workflow.Id,
            secondStep.Id,
            "finance@company.com",
            ApprovalDecision.Approved,
            "Finance approved");

        workflow.Status.Should().Be(WorkflowStatus.InProgress);

        service.TakeAction(
            workflow.Id,
            secondStep.Id,
            "legal@company.com",
            ApprovalDecision.Approved,
            "Legal approved");

        secondStep.Status.Should().Be(StepStatus.Approved);
        workflow.Status.Should().Be(WorkflowStatus.Approved);
    }
}
