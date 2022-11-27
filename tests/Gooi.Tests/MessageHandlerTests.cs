using Moq;
using Newtonsoft.Json;
using Skinder.Gooi.Azure;
using Skinder.Gooi.Contracts.Interfaces.Infrastructure;

namespace Skinder.Gooi.Tests;

public class MessageHandlerTests
{
    public MessageHandlerTests()
    {
        ConfigureEnvironmentVariablesFromLocalSettings();
    }

    [Fact]
    public async Task MessageHandler_Should_Return0_WithBasicSetup()
    {
        var writer = new Mock<IInfrastructureOutputWriter>();
        
        var messageHandler = new AzureMessageCommandHandler(writer.Object);
        var result = await messageHandler.Handle(new AzureQueueMessageCommand()
        {
            Message = "Hello",
            Queue = "demo",
            ConnectionString = "UseDevelopmentStorage=true"
        }, new CancellationToken());
        
        Assert.True(result == 0);
    }
    static void ConfigureEnvironmentVariablesFromLocalSettings()
    {
        var path = Path.GetDirectoryName(typeof(MessageHandlerTests).Assembly.Location);
        var json = File.ReadAllText(Path.Join(path, "local.settings.json"));
        var parsed = Newtonsoft.Json.Linq.JObject.Parse(json).Value<Newtonsoft.Json.Linq.JObject>("Values");

        foreach (var item in parsed)
        {
            Environment.SetEnvironmentVariable(item.Key, item.Value.ToString());
        }
    }
}