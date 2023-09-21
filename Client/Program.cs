using Client;
using Interfaces;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapHub<VolumesHub>(Config.action);

app.Run();