// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using global::Skinder.Gooi.Application;

namespace Skinder.Gooi.Application;
internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var cancellationToken = new CancellationToken();

        var host = CreateHostBuilder().Build();
        return await host.Services.GetRequiredService<Startup>().Execute(args, cancellationToken);
        //AnsiConsole.Markup("[underline red]Hello[/] World!");
    }
    internal static IHostBuilder CreateHostBuilder() 
    {
        return new HostBuilder()
        .ConfigureServices((hostConfig, services) => {
          services.AddScoped<Startup>();
          services.AddScoped<ICommandOptions, DemoCommand>();
          services.AddMediatR(Assembly.GetExecutingAssembly());
        });
    }
}