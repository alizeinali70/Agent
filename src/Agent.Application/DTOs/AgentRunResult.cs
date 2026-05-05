namespace Agent.Application.DTOs;

public sealed record AgentRunResult
{
    public bool Succeeded { get; init; }
    public string? FinalAnswer { get; init; }
    public string? ErrorMessage { get; init; }
    public AgentRunStatus Status { get; init; }

    public static AgentRunResult Completed(string finalAnswer)
    {
        return new AgentRunResult
        {
            Succeeded = true,
            Status = AgentRunStatus.Completed,
            FinalAnswer = finalAnswer
        };
    }

    public static AgentRunResult Failed(string errorMessage)
    {
        return new AgentRunResult
        {
            Succeeded = false,
            Status = AgentRunStatus.Failed,
            ErrorMessage = errorMessage
        };
    }
}