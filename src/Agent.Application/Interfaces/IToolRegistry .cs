using Agent.Domain.Entities;

namespace Agent.Application.Interfaces;

public interface IToolRegistry
{
    IAgentTool GetRequiredTool(string name);

    IReadOnlyCollection<ToolDefinition> GetAvailableTools();
}