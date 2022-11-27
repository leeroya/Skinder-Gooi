namespace Skinder.Gooi.Azure;

[Verb("azure", HelpText = "Creates a azure message.")]
public class MessageCommand: IRequest<int>, IAzureCommandOptions
{
    [Option('m',"message", SetName = "Message", HelpText = "Supply message to be sent to the cloud provider.")]
    public string? Message { get; set; }

    [Option('c',"connection", SetName = "ConnectionString", HelpText = "Supply the cloud provider connection string.")]
    public string? ConnectionString { get; set; }
}
