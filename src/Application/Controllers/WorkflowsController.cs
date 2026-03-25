using Microsoft.AspNetCore.Mvc;
using WorkflowEngineSimulator.Models;
using WorkflowEngineSimulator.Services;

namespace WorkflowEngineSimulator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowsController : ControllerBase
{
    private readonly WorkflowService _workflowService;

    public WorkflowsController(WorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateWorkflowRequest request)
    {
        var workflow = _workflowService.Create(request.Title, request.RequestorEmail);
        return Ok(workflow);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_workflowService.GetAll());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var workflow = _workflowService.GetById(id);
        return workflow == null ? NotFound() : Ok(workflow);
    }

    [HttpPost("{requestId:guid}/steps/{stepId:guid}/action")]
    public IActionResult TakeAction(Guid requestId, Guid stepId, [FromBody] WorkflowActionRequest request)
    {
        var success = _workflowService.TakeAction(
            requestId,
            stepId,
            request.ApproverEmail,
            request.Decision,
            request.Comments);

        return success ? Ok(new { message = "Action recorded successfully." }) : BadRequest(new { message = "Unable to process action." });
    }
}

public class CreateWorkflowRequest
{
    public string Title { get; set; } = string.Empty;
    public string RequestorEmail { get; set; } = string.Empty;
}

public class WorkflowActionRequest
{
    public string ApproverEmail { get; set; } = string.Empty;
    public ApprovalDecision Decision { get; set; }
    public string? Comments { get; set; }
}
