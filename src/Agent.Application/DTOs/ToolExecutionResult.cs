namespace Agent.Application.DTOs;

public sealed record ToolExecutionResult
{
    public bool Succeeded { get; init; }

    public string? Output { get; init; }

    public string? Error { get; init; }

    public IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    public static ToolExecutionResult Success(
        string output,
        IReadOnlyDictionary<string, object?>? metadata = null)
    {
        return new ToolExecutionResult
        {
            Succeeded = true,
            Output = output,
            Metadata = metadata
        };
    }

    public static ToolExecutionResult Failure(string error)
    {
        return new ToolExecutionResult
        {
            Succeeded = false,
            Error = error
        };
    }
}