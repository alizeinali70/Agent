using Agent.Application.DTOs;
using Agent.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Api.Controllers;

[Route("api/agents")]
[ApiController]
public class AgentsController : ControllerBase
{
    private readonly IAgentRunner _agentRunner;

    public AgentsController(IAgentRunner agentRunner)
    {
        _agentRunner = agentRunner;
    }

    [HttpGet]
    public IActionResult Runs()
    {
        return Ok();
    }

    [HttpPost("run")]
    public async Task<ActionResult<AgentRunResult>> RunAsync(
        AgentRunRequest request,
        CancellationToken ct)
    {
        var result = await _agentRunner.RunAsync(request, ct);

        if (!result.Succeeded)
            return BadRequest(result);

        return await Task.FromResult(Ok(result));
    }
}