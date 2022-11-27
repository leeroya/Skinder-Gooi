using Moq;
using Skinder.Gooi.Azure;
using Skinder.Gooi.Contracts.Interfaces.Infrastructure;

namespace Skinder.Gooi.Tests;

public class MessageHandlerTests
{
    [Fact]
    public async Task MessageHandler_Should_Return0_WithBasicSetup()
    {
        var writer = new Mock<IInfrastructureOutputWriter>();
        
        var messageHandler = new AzureMessageCommandHandler(writer.Object);
        //
        var result = await messageHandler.Handle(new AzureQueueMessageCommand()
        {
            Message = "Hello"
        }, new CancellationToken());
        
        Assert.True(result == 0);
    }
}