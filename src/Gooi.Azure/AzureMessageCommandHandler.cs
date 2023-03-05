using Skinder.Gooi.Contracts.Interfaces.Infrastructure;

namespace Skinder.Gooi.Azure;

public class AzureMessageCommandHandler : IRequestHandler<AzureQueueMessageCommand, int>
{
    private readonly IInfrastructureOutputWriter _outputWriter;
    public AzureMessageCommandHandler(IInfrastructureOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }
    public async Task<int> Handle(AzureQueueMessageCommand request, CancellationToken cancellationToken)
    {
        _outputWriter.WriteFaintLine("Azure message handling");
        QueueClient queue = new QueueClient(request.ConnectionString , request.Queue);
        if (string.IsNullOrEmpty(request.Message))
        {
            _outputWriter.WriteError("Message was not passed, nothing was sent.");
            return 1;
        }

        await InsertMessageAsync(queue, request.Message, request.Expire);
        return 0;
    }
    private async Task InsertMessageAsync(QueueClient theQueue, string message, bool expire)
    {
        if (null != await theQueue.CreateIfNotExistsAsync())
        {
            _outputWriter.WriteLine("The queue was created.");
        }

        var encodedMessage = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));

        if (expire)
        {
            var messageLifeSpan = TimeSpan.FromSeconds(-1);
            await theQueue.SendMessageAsync(encodedMessage, default,messageLifeSpan,default);
            return;
        }
        await theQueue.SendMessageAsync(encodedMessage);
    }
}