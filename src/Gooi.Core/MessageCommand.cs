using MediatR;
using Skinder.Gooi.Core.Options;

namespace Skinder.Gooi.Core;
public class MessageCommand: IRequest<int>, ICommandOptions
{
  [Option("Message", HelpText = "Provide a string message that can be sent.")]
  public string Message { get; set; } 
}
