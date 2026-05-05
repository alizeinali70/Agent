using System.Text.Json;
using Agent.Application.DTOs;
using Agent.Application.Interfaces;
using OpenAI.Chat;

namespace Agent.Application.Agents;

public sealed class OpenAiAgentPlanner : IAgentPlanner
{
    private readonly ChatClient _chatClient;
    private readonly IToolRegistry _toolRegistry;

    public OpenAiAgentPlanner(
        ChatClient chatClient,
        IToolRegistry toolRegistry)
    {
        _chatClient = chatClient;
        _toolRegistry = toolRegistry;
    }

    public async Task<AgentPlan> CreatePlanAsync(
        AgentContext context,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(context);

        var options = new ChatCompletionOptions();

        foreach (var tool in BuildToolDefinitions())
        {
            options.Tools.Add(tool);
        }

        var response = await _chatClient.CompleteChatAsync(
            BuildMessages(context),
            options,
            ct);

        var message = response.Value.Content.Count > 0
            ? response.Value
            : throw new InvalidOperationException("OpenAI returned an empty response.");

        var assistantText = GetText(message);

        if (message.ToolCalls is { Count: > 0 })
        {
            var toolCall = message.ToolCalls[0];

            var input = JsonSerializer.Deserialize<Dictionary<string, object?>>(
                            toolCall.FunctionArguments.ToString())
                        ?? new Dictionary<string, object?>();

            return AgentPlan.Tool(
                new ToolCall(toolCall.FunctionName, input),
                assistantText);
        }

        return AgentPlan.Final(assistantText);
    }

    private static List<ChatMessage> BuildMessages(AgentContext context)
    {
        var messages = new List<ChatMessage>
        {
            ChatMessage.CreateSystemMessage("""
                                            You are an agent.
                                            Decide the next step.

                                            Rules:
                                            - If you need data, call a tool.
                                            - If you can answer, respond directly.
                                            - Be concise.
                                            """),

            ChatMessage.CreateUserMessage(context.Goal)
        };

        foreach (var step in context.Steps)
        {
            messages.Add(ChatMessage.CreateAssistantMessage(
                $"Tool '{step.ToolName}' returned: {step.ToolOutput ?? "(no output)"}"));
        }

        return messages;
    }

    private List<ChatTool> BuildToolDefinitions()
    {
        return _toolRegistry
            .GetAvailableTools()
            .Select(tool => ChatTool.CreateFunctionTool(
                tool.Name,
                tool.Description,
                tool.JsonSchema))
            .ToList();
    }

    private static string GetText(ChatCompletion completion)
    {
        return string.Join(
            "",
            completion.Content.Select(part => part.Text));
    }
}