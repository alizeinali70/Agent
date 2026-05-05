namespace Agent.Application.DTOs;

public sealed record AgentPlan
{
    public bool IsFinal { get; init; }

    public string? FinalAnswer { get; init; }

    public ToolCall? ToolCall { get; init; }

    public string? ThoughtSummary { get; init; }

    public static AgentPlan Final(string answer)
    {
        return new AgentPlan
        {
            IsFinal = true,
            FinalAnswer = answer
        };
    }

    public static AgentPlan Tool(ToolCall call, string? thought = null)
    {
        return new AgentPlan
        {
            IsFinal = false,
            ToolCall = call,
            ThoughtSummary = thought
        };
    }
}