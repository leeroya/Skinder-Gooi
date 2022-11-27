using Skinder.Gooi.Contracts.Interfaces.Options;


namespace Skinder.Gooi.Tests;

public class ConnectionProperties: IConnectionProperties
{
  public string ConnectionString { get; set; }
}