using MediatR;
using Skinder.Gooi.Core.Infrastructure;
using Skinder.Gooi.Core.Options;

namespace Skinder.Gooi.Domain.Features.AzureMessage;

public class MessageHandler: IRequestHandler<MessageCommand, int>
{
    private readonly IConnectionProperties _connectionProperties;
    private readonly IOutputWriter _outputWriter;
  
    public MessageHandler(IOutputWriter outputWriter, IConnectionProperties connectionProperties)
    {
      _outputWriter = outputWriter;
      _connectionProperties = connectionProperties;
    }

    public async Task<int> Handle(MessageCommand request, CancellationToken cancellationToken)
    {
      Console.WriteLine("Woop woop yay!");
      
      return 0;
    }
}
