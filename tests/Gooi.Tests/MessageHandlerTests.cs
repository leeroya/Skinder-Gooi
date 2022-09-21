using Moq;
using Skinder.Gooi.Azure;
using Skinder.Gooi.Core;
using Skinder.Gooi.Core.Infrastructure;

namespace Skinder.Gooi.Tests;

public class MessageHandlerTests
{
    [Fact]
    public async Task MessageHandler_Should_Return0_WithBasicSetup()
    {
        var writer = new Mock<IOutputWriter>();
        var connectionProperties = new ConnectionProperties { ConnectionString = "Foo"};

        var messageHandler = new MessageHandler(writer.Object, connectionProperties);
        //
        var result = await messageHandler.Handle(new MessageCommand
        {
            Message = "Hello"
        }, new CancellationToken());
        
        Assert.True(result == 0);
    }
}