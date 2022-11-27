
using Skinder.Gooi.Azure;
using Skinder.Gooi.Contracts.Interfaces.Infrastructure;
using Skinder.Gooi.Contracts.Interfaces.Options;
using ILogger = Microsoft.Extensions.Logging.ILogger;

[assembly: InternalsVisibleTo("Skinder.Gooi.Tests")]

namespace Skinder.Gooi.CLI;
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
        services.AddAutoMapper(typeof(Profile));
        services.Scan(c =>
        {
          c.FromAssemblyOf<ICommandOptions>()
            .AddClasses(x => x.NotInNamespaceOf(typeof(RequestPerformanceBehaviour<,>))
              .Where(type => type.Namespace != null && type.Namespace.Contains("Skinder.Gooi")))
            .AsImplementedInterfaces()
            .WithTransientLifetime();
          
          c.FromAssemblyOf<IAzureCommandOptions>()
            .AddClasses(x => x.NotInNamespaceOf(typeof(RequestPerformanceBehaviour<,>))
              .Where(type => type.Namespace != null && type.Namespace.Contains("Gooi")))
            .AsImplementedInterfaces()
            .WithTransientLifetime();
        });

        services.AddSingleton<Startup>();

        services.AddSingleton<IInfrastructureOutputWriter, AnsiOutputWriter>();
        services.AddMediatR(typeof(Startup));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
        
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
      _logger?.LogError("{Message}", ex.Message);

      errorBuilder.AppendLine($"Error: {ex.Message}");

      if (ex is FluentValidation.ValidationException error)
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