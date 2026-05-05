namespace Agent.Domain.Entities;

public sealed record AgentStep(
    int StepNumber,
    string ToolName,
    IReadOnlyDictionary<string, object?> ToolInput,
    string? ToolOutput,
    bool Success);