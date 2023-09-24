using Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((ctx, builder) =>
    {
        builder.AddConsole();
        builder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
        builder.AddFile(o => o.RootPath = ctx.HostingEnvironment.ContentRootPath);
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<VolumesHubClient>();
    })
    .Build();

host.Run();

