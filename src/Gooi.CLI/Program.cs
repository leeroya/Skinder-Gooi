using System.ComponentModel.DataAnnotations;
using System.Text;
using MediatR;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Gooi.Core.Infrastructure;
using Gooi.Core.Options;
using Serilog.Events;
using FluentValidation.Results;

namespace Gooi.CLI;
class Program
{
  private static IInfrastructureOutputWriter? _outputWriter;
  private static ILogger? _logger;
  private static bool _debugMode;
  internal static async Task<int> Main(string[] args)
  {
    var cancellationToken = new CancellationToken();
    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Information()
      .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "azdo-data.log"), LogEventLevel.Information)
      .Enrich.FromLogContext()
      .CreateLogger();

    _debugMode = Array.Exists(args, x => x.Equals("::debug"));

    var host = CreateHostBuilder().Build();

    _outputWriter = host.Services.GetRequiredService<IInfrastructureOutputWriter>();
    _logger = host.Services.GetRequiredService<ILogger<Program>>();
    return await host.Services.GetRequiredService<Startup>().Execute(args, cancellationToken);
  }

  internal static IHostBuilder CreateHostBuilder()
  {
    return new HostBuilder()
      .ConfigureHostConfiguration(config =>
      {
        config.SetBasePath(Path.Combine(AppContext.BaseDirectory));
        config.AddJsonFile("appSettings.json", true);
      }
      )
      .ConfigureServices((hostConfig, services) =>
      {
        /*services.SetupPushFeature();
        services.SetupPullRequestFeature();
        services.SetupCommitFeature();
        services.SetupProjectFeature();
        services.SetupCodeRepositoryFeature();
        services.AddAutoMapper(typeof(AutoMapperProfile));*/
        services.Scan(c =>
        {
          c.FromAssemblyOf<ICommandOptions>()
            .AddClasses(x => x.NotInNamespaceOf(typeof(RequestPerformanceBehaviour<,>))
              .Where(type => type.Namespace != null && type.Namespace.Contains("Internal.SDLC")))
            .AsImplementedInterfaces()
            .WithTransientLifetime();
        });

        services.AddSingleton<Startup>();

        services.AddSingleton<IInfrastructureOutputWriter, AnsiOutputWriter>();
        services.AddMediatR(typeof(Startup));

        /*        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehaviour<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));*/
      })
      .ConfigureLogging((hostingContext, options) =>
      {
        options.ClearProviders();
        options.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        options.AddSerilog(Log.Logger, dispose: true);
      });
  }

  public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
  {
    StringBuilder errorBuilder = new StringBuilder();
    errorBuilder.AppendLine("An error has occurred while trying to execute this command:");

    if (e.ExceptionObject is Exception ex)
    {
      if (_logger != null) _logger.LogError("{Message}", ex.Message);

      errorBuilder.AppendLine($"Error: {ex.Message}");

      if (ex is ValidationException error)
      {
        foreach (ValidationFailure validationFailure in error.Errors)
        {
          errorBuilder.AppendLine($"    {validationFailure.ErrorMessage}");
        }
      }
      else if (ex.InnerException != null)
      {
        errorBuilder.AppendLine($"    {ex.InnerException.Message}");
      }
    }

    _outputWriter?.WriteError(errorBuilder.ToString());

    if (_debugMode)
    {
      _outputWriter?.WriteException(e.ExceptionObject as Exception);
    }

    Log.CloseAndFlush();
    Environment.Exit(1);
  }
}