using ConsoleTableExt;
using Microsoft.Extensions.Logging;
using Skinder.Gooi.Contracts.Interfaces.Infrastructure;
using Spectre.Console;

namespace Skinder.Gooi.Core.Infrastructure;
public class AnsiOutputWriter : IInfrastructureOutputWriter
{
  private const string ColourHeading1 = "deepskyblue1";
  private const string ColourHeading2 = "deepskyblue1";
  private const string ColourHeading3 = "deepskyblue1";
  private const string ColourCompletedMessage = "deepskyblue1";
  private readonly ILogger _logger;

  public AnsiOutputWriter(ILogger<AnsiOutputWriter> logger)
  {
    _logger = logger;
  }

  public void Write(string value)
  {
    _logger.LogInformation(value);
    AnsiConsole.Write(value.EscapeMarkup());
  }

  public void WriteLine(string value)
  {
    _logger.LogInformation(value);
    AnsiConsole.WriteLine(value);
  }

  public void WriteFaintLine(string value)
  {
    _logger.LogDebug(value);
    AnsiConsole.MarkupLine("[dim]{0}[/]", value.EscapeMarkup());
  }

  public void WriteFile(string filename, string content)
  {
    _logger.LogInformation($"File: {filename}");
    _logger.LogInformation(content);

    AnsiConsole.WriteLine(Environment.NewLine);
    AnsiConsole.MarkupLine($"[yellow]File: {filename}[/]");
    AnsiConsole.MarkupLine("[cyan]{0}[/]", content.EscapeMarkup());
    AnsiConsole.WriteLine(Environment.NewLine);
  }

  public void WriteHeading(string value, HeadingLevel level1 = HeadingLevel.Level2)
  {
    AnsiConsole.Write(Environment.NewLine);

    switch (level1)
    {
      case HeadingLevel.Level1:
        {
          AnsiConsole.Write(new Rule($"{Emoji.Known.Detective} [{ColourHeading1}]{value.EscapeMarkup()}[/]").LeftAligned());
          break;
        }

      case HeadingLevel.Level2:
        {
          AnsiConsole.Write(new Markup($"[{ColourHeading2}]==> {value.EscapeMarkup()}[/][white]:[/]"));
          break;
        }

      case HeadingLevel.Level3:
        AnsiConsole.Write(new Markup($"[{ColourHeading3}]{value.EscapeMarkup()}[/]"));
        break;

      default:
        throw new ArgumentOutOfRangeException(nameof(level1), level1, null);
    }

    AnsiConsole.Write(Environment.NewLine);
  }

  public void WriteContent(string content)
  {
    _logger.LogInformation(content);
    AnsiConsole.MarkupLine("[cyan]{0}[/]", content.EscapeMarkup());
  }

  public void WriteCompletedMessage(string value)
  {
    _logger.LogInformation(value);
    AnsiConsole.Write(Environment.NewLine);
    AnsiConsole.MarkupLine("[{0}]:ok_hand: {1}[/]", ColourCompletedMessage, value.EscapeMarkup());
    AnsiConsole.Write(Environment.NewLine);
  }

  public void WriteSuccess(string value)
  {
    AnsiConsole.MarkupLine("[lime]\u221A {0}[/]", value.EscapeMarkup());
  }

  public void WriteTable<T>(List<T> list) where T : class
  {
    AnsiConsole.Write(Environment.NewLine);
    if (list != null && list.Any())
    {
      ConsoleTableBuilder.From(list).WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();
    }

    AnsiConsole.Write(Environment.NewLine);
  }

  public void WriteWarning(string value)
  {
    _logger.LogWarning(value);
    AnsiConsole.MarkupLine("[orange1]{0}[/]", value.EscapeMarkup());
  }

  public void WriteError(string value)
  {
    _logger.LogError(value);
    AnsiConsole.MarkupLine("[red]:disappointed_face: {0}[/]", value.EscapeMarkup());
  }

  public void WriteException(Exception? ex)
  {
    if (ex != null)
    {
      AnsiConsole.WriteException(ex);
    }
  }

  public Tree CreateTree(string title)
  {
    return new Tree(title);
  }

  public void WriteTree(Tree root)
  {
    AnsiConsole.Write(root);
  }
}
