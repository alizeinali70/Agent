using Agent.Application.Agents;
using Agent.Application.Interfaces;
using Agent.Application.Tools;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAgentRunner, AgentRunner>();
builder.Services.AddScoped<IAgentPlanner, OpenAiAgentPlanner>();
builder.Services.AddScoped<IAgentTool, EchoTool>();
builder.Services.AddScoped<IToolRegistry, ToolRegistry>();

builder.Services.AddSingleton(_ =>
{
    var apiKey = builder.Configuration["OpenAI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new InvalidOperationException("Missing OpenAI:ApiKey configuration.");

    return new ChatClient("gpt-4.1-mini", apiKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();