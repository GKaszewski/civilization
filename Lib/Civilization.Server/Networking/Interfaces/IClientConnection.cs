using Civilization.Shared.Packets.ServerMessages;

namespace Civilization.Server.Networking.Interfaces;

public interface IClientConnection
{
    int PlayerId { get; }
    Task SendAsync(BaseServerMessage message);
}