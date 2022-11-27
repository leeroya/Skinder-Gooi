namespace Skinder.Gooi.Contracts.Interfaces.Managers;

public interface IMessageManager
{
    Task<bool> Gooi(string message);
}