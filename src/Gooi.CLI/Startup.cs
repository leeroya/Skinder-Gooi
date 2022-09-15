using CommandLine;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace Skinder.Gooi.CLI;
public class Startup
{
  private readonly IHost _host;
  private readonly IMediator _mediator;
  private readonly IInfrastructureOutputWriter _outputWriter;
  public Startup(IMediator mediator, IInfrastructureOutputWriter outputWriter, IHost host)
  {
    _mediator = mediator;
    _outputWriter = outputWriter;
    _host = host;
  }

  public async Task<int> Execute(string[] args, CancellationToken cancellationToken = default)
  {
    _outputWriter.WriteHeading($"Gooi weergawe is: {GetType().Assembly.GetName().Version?.ToString()}", HeadingLevel.Level1);

    Parsed<object>? result = ParseArguments(args);
    if (result != null && result.Value is ICommandOptions command)
    {
      if (command is IConnectionProperties properties)
      {
        //_host.Services.GetRequiredService<IDevOpsDataWarehouseRepository>().SetConnectionString(properties);
      }

      await _mediator.Send(command, cancellationToken);
      _outputWriter.WriteCompletedMessage($"{Emoji.Known.Detective}  Henchman is done!");
      return 0;
    }

    if (args.Contains("--help") || args.Contains("--version"))
    {
      return 0;
    }

    return 1;
  }
  private Parsed<object>? ParseArguments(string[] args)
  {
    var parser = new Parser(settings =>
    {
      settings.EnableDashDash = true;
      settings.CaseSensitive = false;
      settings.HelpWriter = System.Console.Out;
    });

    List<ICommandOptions> commands = _host.Services.GetServices<ICommandOptions>().ToList();

    if (!commands.Any())
    {
      throw new InvalidOperationException($" {Emoji.Known.WorriedFace} Well this is embarrassing. Henchman does not know what to do.");
    }

    Type[] types = commands.Select(s => s.GetType()).ToArray();
    var result = parser.ParseArguments(args, types) as Parsed<object>;

    return result;
  }
}
