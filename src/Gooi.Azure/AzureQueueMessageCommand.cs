namespace Skinder.Gooi.Azure;

[Verb("azure-queue", HelpText = "Creates a azure message.")]
public class AzureQueueMessageCommand: IRequest<int>, IAzureCommandOptions
{
    [Option("message", SetName = "Azure-Queue", HelpText = "Message to be sent to queue.")]
    public string? Message { get; set; }

    [Option("expire", SetName = "Azure-Queue", HelpText = "Message to be sent to queue.")]
    public bool Expire { get; set; } = false;
    
    [Option("queue", SetName = "Azure-Queue", HelpText = "Desired queue to send the message.")]
    public string? Queue { get; set; }

    [Option("connection", SetName = "Azure-Queue", HelpText = "Azure queue connection string.")]
    public string? ConnectionString { get; set; }
}
