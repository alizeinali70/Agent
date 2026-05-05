using Agent.Domain.Entities;

namespace Agent.Application.DTOs;

public sealed class AgentContext
{
    private readonly List<AgentStep> _steps = new();

    private readonly List<ToolExecutionResult> _toolResults = new();
    public Guid RunId { get; }
    public Guid UserId { get; }
    public string Goal { get; }
    public IReadOnlyList<AgentStep> Steps => _steps;
    public IReadOnlyList<ToolExecutionResult> ToolResults => _toolResults;

    public AgentExecutionContext Execution { get; }

    public AgentContext(
        Guid runId,
        Guid userId,
        string goal,
        AgentExecutionContext execution)
    {
        RunId = runId;
        UserId = userId;
        Goal = goal;
        Execution = execution;
    }

    public void AddStep(AgentStep step)
    {
        _steps.Add(step);
    }

    public void AddToolResult(
        ToolCall call,
        ToolExecutionResult result)
    {
        ArgumentNullException.ThrowIfNull(call);
        ArgumentNullException.ThrowIfNull(result);

        _toolResults.Add(result);

        _steps.Add(new AgentStep(
            _steps.Count + 1,
            call.Name,
            call.Input,
            result.Output,
            result.Succeeded));
    }
}