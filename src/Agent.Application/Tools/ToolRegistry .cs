using Agent.Application.Interfaces;
using Agent.Domain.Entities;

namespace Agent.Application.Tools;

public sealed class ToolRegistry : IToolRegistry
{
    private readonly List<ToolDefinition> _definitions;
    private readonly Dictionary<string, IAgentTool> _tools;

    public ToolRegistry(IEnumerable<IAgentTool> tools)
    {
        var toolList = tools.ToList(); // materialize once

        _tools = toolList.ToDictionary(
            t => t.Name,
            StringComparer.OrdinalIgnoreCase);

        _definitions = toolList
            .Select(BuildDefinition)
            .ToList();
    }

    public IAgentTool GetRequiredTool(string name)
    {
        if (!_tools.TryGetValue(name, out var tool))
        {
            throw new InvalidOperationException(
                $"Tool '{name}' not registered.");
        }

        return tool;
    }

    public IReadOnlyCollection<ToolDefinition> GetAvailableTools()
    {
        return _definitions;
    }

    private static ToolDefinition BuildDefinition(IAgentTool tool)
    {
        var schema = new
        {
            type = "object",
            properties = new
            {
                message = new
                {
                    type = "string",
                    description = "Message to echo"
                }
            },
            required = new[] { "message" }
        };

        return new ToolDefinition(
            tool.Name,
            "Echoes a message back",
            BinaryData.FromObjectAsJson(schema));
    }
}