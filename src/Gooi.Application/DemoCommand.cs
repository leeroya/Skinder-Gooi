using CommandLine;
using MediatR;

namespace Gooi.Application;

[Verb("demo", HelpText = "Skippy was here.")]
public class DemoCommand : IRequest<int>, ICommandOptions
{

  [Option("attributes", HelpText = "Provide a list of attributes in the form test1=value1;test2=value2;.")]
  public string Attributes { get; set; }

}