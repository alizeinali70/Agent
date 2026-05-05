using Agent.Application.DTOs;

namespace Agent.Application.Interfaces;

public interface IAgentPlanner
{
    Task<AgentPlan> CreatePlanAsync(AgentContext context, CancellationToken ct);
}