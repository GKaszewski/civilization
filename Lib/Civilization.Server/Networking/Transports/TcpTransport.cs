using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Civilization.Server.Game;
using Civilization.Server.Networking.Interfaces;
using Civilization.Shared;
using Civilization.Shared.Packets;

namespace Civilization.Server.Networking.Transports;

public class TcpTransport : ITransport
{
    private readonly int _port;

    public TcpTransport(int port = 9000)
    {
        _port = port;
    }
    
    public async Task StartAsync(GameSessionCoordinator coordinator, CancellationToken cancellationToken = default)
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();
        Console.WriteLine($"TCP transport listening on port {_port}");

        var nextPlayerId = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(cancellationToken);
            _ = HandleClientAsync(client, nextPlayerId++, coordinator, cancellationToken);
        }
    }
    
    private async Task HandleClientAsync(
        TcpClient client,
        int playerId,
        GameSessionCoordinator coordinator,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Client {playerId} connected.");
        var stream = client.GetStream();
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream) { AutoFlush = true };

        await writer.WriteLineAsync($"{{\"info\": \"You are Player {playerId}\"}}");
        
        var connection = new TcpClientConnection(playerId, stream);

        await coordinator.RegisterPlayerAsync(playerId, connection);

        while (!cancellationToken.IsCancellationRequested && client.Connected)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
                break;

            try
            {
                var message = JsonSerializer.Deserialize<ClientMessage>(line, SharedJson.Options);
                if (message is not null)
                {
                    await coordinator.ReceiveCommandAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Player {playerId}] Failed to process message: {ex.Message}");
            }
        }

        Console.WriteLine($"Client {playerId} disconnected.");
    }
}