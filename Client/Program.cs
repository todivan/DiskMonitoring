using Client;
using Interfaces;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapHub<VolumesHub>(Config.action);

app.Run();