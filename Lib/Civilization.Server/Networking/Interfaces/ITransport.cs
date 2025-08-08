using Civilization.Server.Game;

namespace Civilization.Server.Networking.Interfaces;

public interface ITransport
{
    Task StartAsync(GameSessionCoordinator coordinator, CancellationToken cancellationToken = default);
}