namespace Agent.Application.DTOs;

public sealed class AgentExecutionContext
{
    public IServiceProvider Services { get; }
    public DateTime UtcNow { get; }

    public AgentExecutionContext(
        IServiceProvider services,
        DateTime utcNow)
    {
        Services = services;
        UtcNow = utcNow;
    }
}