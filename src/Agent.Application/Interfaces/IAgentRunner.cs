using Agent.Application.DTOs;

namespace Agent.Application.Interfaces;

public interface IAgentRunner
{
    Task<AgentRunResult> RunAsync(AgentRunRequest request, CancellationToken ct);
}