using Agent.Application.DTOs;
using Agent.Application.Interfaces;

namespace Agent.Application.Agents;

public sealed class AgentRunner : IAgentRunner
{
    private const int MaxSteps = 5;

    private readonly IAgentPlanner _planner;
    private readonly IServiceProvider _services;
    private readonly IToolRegistry _toolRegistry;

    public AgentRunner(
        IAgentPlanner planner,
        IToolRegistry toolRegistry,
        IServiceProvider services)
    {
        _planner = planner;
        _toolRegistry = toolRegistry;
        _services = services;
    }

    public async Task<AgentRunResult> RunAsync(
        AgentRunRequest request,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Goal))
            return AgentRunResult.Failed("Goal must be provided.");

        var context = new AgentContext(
            Guid.NewGuid(),
            request.UserId,
            request.Goal,
            new AgentExecutionContext(
                _services,
                DateTime.UtcNow));

        for (var step = 0; step < MaxSteps; step++)
        {
            ct.ThrowIfCancellationRequested();

            var plan = await _planner.CreatePlanAsync(context, ct);

            if (plan.IsFinal)
            {
                return AgentRunResult.Completed(
                    plan.FinalAnswer ?? string.Empty);
            }

            if (plan.ToolCall is null)
            {
                return AgentRunResult.Failed(
                    "Planner did not return a final answer or a tool call.");
            }

            var tool = _toolRegistry.GetRequiredTool(plan.ToolCall.Name);

            var toolResult = await tool.ExecuteAsync(
                plan.ToolCall.Input,
                context.Execution,
                ct);

            context.AddToolResult(plan.ToolCall, toolResult);

            if (!toolResult.Succeeded)
            {
                return AgentRunResult.Failed(
                    toolResult.Error ?? $"Tool '{plan.ToolCall.Name}' failed.");
            }
        }

        return AgentRunResult.Failed(
            $"Agent reached maximum step limit of {MaxSteps}.");
    }
}