using Agent.Application.DTOs;
using Agent.Application.Interfaces;

namespace Agent.Application.Agents;

public sealed class FakeAgentRunner : IAgentRunner
{
    public Task<AgentRunResult> RunAsync(
        AgentRunRequest request,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(
            AgentRunResult.Completed($"Received goal: {request.Goal}"));
    }
}