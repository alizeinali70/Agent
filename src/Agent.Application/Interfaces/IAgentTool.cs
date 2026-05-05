using Agent.Application.DTOs;

namespace Agent.Application.Interfaces;

public interface IAgentTool
{
    string Name { get; }

    Task<ToolExecutionResult> ExecuteAsync(
        IReadOnlyDictionary<string, object?> input,
        AgentExecutionContext context,
        CancellationToken ct);
}