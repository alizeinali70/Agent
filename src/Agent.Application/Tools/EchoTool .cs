using Agent.Application.DTOs;
using Agent.Application.Interfaces;

namespace Agent.Application.Tools;

public sealed class EchoTool : IAgentTool
{
    public string Name => "echo";

    public Task<ToolExecutionResult> ExecuteAsync(
        IReadOnlyDictionary<string, object?> input,
        AgentExecutionContext context,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(input);

        var message = input.TryGetValue("message", out var value)
            ? value?.ToString()
            : null;

        if (string.IsNullOrWhiteSpace(message))
        {
            return Task.FromResult(
                ToolExecutionResult.Failure("Missing 'message' input."));
        }

        return Task.FromResult(
            ToolExecutionResult.Success($"Echo: {message}"));
    }
}