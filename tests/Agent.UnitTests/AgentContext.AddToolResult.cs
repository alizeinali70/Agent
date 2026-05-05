using Agent.Api.Controllers;
using Agent.Application.DTOs;
using Agent.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Agent.UnitTests;

public class AgentContextAddToolResult
{
    [Fact]
    public void AddToolResultWhenResultIsNullThrowsArgumentNullException()
    {
        var context = new AgentContext(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test goal",
            new AgentExecutionContext(Mock.Of<IServiceProvider>(), DateTime.UtcNow));

        var call = new ToolCall("test-tool", new Dictionary<string, object?>());

        var act = () => context.AddToolResult(call, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void AddToolResultWhenValidAddsStepAndToolResult()
    {
        var context = new AgentContext(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test goal",
            new AgentExecutionContext(Mock.Of<IServiceProvider>(), DateTime.UtcNow));

        var call = new ToolCall(
            "search",
            new Dictionary<string, object?>
            {
                ["query"] = "test"
            });

        var result = ToolExecutionResult.Success("Found data");

        context.AddToolResult(call, result);

        context.ToolResults.Should().ContainSingle();
        context.Steps.Should().ContainSingle();

        var step = context.Steps.Single();

        step.StepNumber.Should().Be(1);
        step.ToolName.Should().Be("search");
        step.ToolOutput.Should().Be("Found data");
        step.Success.Should().BeTrue();
    }

    [Fact]
    public async Task RunWhenAgentSucceedsReturnsOkAsync()
    {
        var runner = new Mock<IAgentRunner>();

        runner
            .Setup(x => x.RunAsync(It.IsAny<AgentRunRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AgentRunResult.Completed("Done"));

        var controller = new AgentsController(runner.Object);

        var request = new AgentRunRequest(
            Guid.NewGuid(),
            "Test goal");

        var response = await controller.RunAsync(request, CancellationToken.None);

        var ok = response.Result.Should().BeOfType<OkObjectResult>().Subject;
        var body = ok.Value.Should().BeOfType<AgentRunResult>().Subject;

        body.Succeeded.Should().BeTrue();
        body.FinalAnswer.Should().Be("Done");
    }

    [Fact]
    public async Task RunWhenAgentFailsReturnsBadRequestAsync()
    {
        var runner = new Mock<IAgentRunner>();

        runner
            .Setup(x => x.RunAsync(It.IsAny<AgentRunRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AgentRunResult.Failed("Something failed"));

        var controller = new AgentsController(runner.Object);

        var request = new AgentRunRequest(
            Guid.NewGuid(),
            "Test goal");

        var response = await controller.RunAsync(request, CancellationToken.None);

        response.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}