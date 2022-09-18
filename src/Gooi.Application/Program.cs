// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

using global::Gooi.Application;

using MediatR.Pipeline;

namespace Gooi.Application;
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
            services.AddMediatR(Assembly.GetExecutingAssembly());
        });
    }
}