namespace Skinder.Gooi.Azure;

[Verb("azure-eventhub", HelpText = "Creates a azure eventhub message.")]
public class AzureEventHubMessageCommand: IRequest<int>, IAzureCommandOptions
{
    [Option("message", SetName = "Azure-EventHub", HelpText = "Message to be sent to queue.")]
    public string? Message { get; set; }

    [Option("queue", SetName = "Azure-EventHub", HelpText = "Desired queue to send the message.")]
    public string? Queue { get; set; }

    [Option("connection", SetName = "Azure-EventHub", HelpText = "Azure queue connection string.")]
    public string? ConnectionString { get; set; }
}