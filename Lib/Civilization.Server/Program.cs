using Civilization.Server.Game;
using Civilization.Server.Networking.Transports;

var transport = new TcpTransport();
var coordinator = new GameSessionCoordinator();

await transport.StartAsync(coordinator);