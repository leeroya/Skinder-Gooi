using CommandLine;

namespace Skinder.Gooi.Core.Options
{
  public interface ICommandOptions
  {
    [Option("provider", SetName = "Provider", HelpText = "Supply the cloud provider needed for sending a message.")]
    public string Provider { get; set; }

    [Option("message", SetName = "Message", HelpText = "Supply message to be sent to the cloud provider.")]
    public string Message { get; set; }

    [Option("connection", SetName = "ConnectionString", HelpText = "Supply the cloud provider connection string.")]
    public string ConnectionString { get; set; }
  }
}
