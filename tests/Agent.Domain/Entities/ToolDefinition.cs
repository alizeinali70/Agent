namespace Agent.Domain.Entities;

public sealed record ToolDefinition(
    string Name,
    string Description,
    BinaryData JsonSchema);