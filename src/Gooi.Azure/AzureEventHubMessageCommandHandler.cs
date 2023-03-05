using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Skinder.Gooi.Contracts.Interfaces.Infrastructure;

namespace Skinder.Gooi.Azure;

public class AzureEventHubMessageCommandHandler : IRequestHandler<AzureEventHubMessageCommand, int>
{
    private readonly IInfrastructureOutputWriter _outputWriter;
    public AzureEventHubMessageCommandHandler(IInfrastructureOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }
    public async Task<int> Handle(AzureEventHubMessageCommand request, CancellationToken cancellationToken)
    {
        _outputWriter.WriteFaintLine("Azure message handling");
        EventHubProducerClient client = new EventHubProducerClient(request.ConnectionString , request.Queue);
        if (string.IsNullOrEmpty(request.Message))
        {
            _outputWriter.WriteError("Message was not passed, nothing was sent.");
            return 1;
        }

        await InsertMessageAsync(client, request.Message);
        return 0;
    }
    private async Task InsertMessageAsync(EventHubProducerClient theQueue, string message)
    {
        // Create a batch of events 
        using EventDataBatch eventBatch = await theQueue.CreateBatchAsync();

        var encodedMessage = Encoding.UTF8.GetBytes(message);
        if (!eventBatch.TryAdd(new EventData(encodedMessage)))
        {
            throw new Exception($"Event message is too large for the batch and cannot be sent.");
        }
        try
        {
            await theQueue.SendAsync(eventBatch);
        }
        finally
        {
            await theQueue.DisposeAsync();
        }
    }
}