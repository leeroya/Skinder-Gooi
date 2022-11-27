using Skinder.Gooi.Contracts.Interfaces.Infrastructure;

namespace Skinder.Gooi.Azure;

public class MessageCommandHandler: IRequestHandler<MessageCommand, int>
{
    private readonly IInfrastructureOutputWriter _outputWriter;
    public MessageCommandHandler(IInfrastructureOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }

    public async Task<int> Handle(MessageCommand request, CancellationToken cancellationToken)
    {
        _outputWriter.WriteFaintLine("Azure message handling");
        return 0;
    }
}