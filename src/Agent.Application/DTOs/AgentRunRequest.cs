namespace Agent.Application.DTOs;

public sealed record AgentRunRequest(
    Guid UserId,
    string Goal,
    IReadOnlyDictionary<string, object>? Inputs = null);