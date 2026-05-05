using Agent.Application.DTOs;

namespace Agent.Application.Interfaces;

public interface IToolExecutor
{
    Task<ToolExecutionResult> ExecuteAsync(ToolCall call, CancellationToken ct);
}