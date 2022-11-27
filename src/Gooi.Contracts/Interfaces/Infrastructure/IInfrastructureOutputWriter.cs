using Spectre.Console;

namespace Skinder.Gooi.Contracts.Interfaces.Infrastructure;
public interface IInfrastructureOutputWriter
{
  Tree CreateTree(string title);
  void WriteTree(Tree root);
  void Write(string value);
  void WriteLine(string value);
  void WriteFaintLine(string value);
  void WriteFile(string filename, string content);
  void WriteHeading(string value, HeadingLevel level1 = HeadingLevel.Level2);
  void WriteContent(string content);
  void WriteTable<T>(List<T> list) where T : class;
  void WriteCompletedMessage(string value);
  void WriteSuccess(string value);
  void WriteWarning(string value);
  void WriteError(string value);
  void WriteException(Exception? ex);
}
