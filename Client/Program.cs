using Client;
using InfrastrucutreModul;
using InfrastrucutreModul.Models;
using Interfaces;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IScanner, Scanner>();
builder.Services.AddSingleton<DiskPartitioWmiScanner>();
builder.Services.AddSingleton<VolumesWinApiScanner>();
builder.Services.AddSingleton<VolumesWmiScanner>();
builder.Services.AddSingleton<DiskPartitioWmiScanner>();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapHub<VolumesHub>(Config.action);

app.Run();