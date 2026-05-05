namespace Agent.Application.DTOs;

public sealed record ToolCall(
    string Name,
    IReadOnlyDictionary<string, object?> Input);