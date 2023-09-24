using Client;
using Common;
using Common.Interfaces;
using InfrastrucutreModul;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddFile(o => o.RootPath = AppContext.BaseDirectory);

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IScanner, Scanner>();
builder.Services.AddSingleton<IDiskPartitionWmiScanner, DiskPartitionWmiScanner>();
builder.Services.AddSingleton<IVolumesWinApiScanner, VolumesWinApiScanner>();
builder.Services.AddSingleton<IVolumesWmiScanner, VolumesWmiScanner>();


var app = builder.Build();

app.MapHub<VolumesHub>(Config.action);

app.Run();