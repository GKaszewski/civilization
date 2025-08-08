using System.Text.Json;
using Civilization.Server.Networking.Interfaces;
using Civilization.Shared;
using Civilization.Shared.Packets.ServerMessages;

namespace Civilization.Server.Networking;

public class TcpClientConnection : IClientConnection
{
    private readonly StreamWriter _writer;
    
    public int PlayerId { get; }

    public TcpClientConnection(int playerId, Stream stream)
    {
        PlayerId = playerId;
        _writer = new StreamWriter(stream) { AutoFlush = true };
    }
    
    public Task SendAsync(BaseServerMessage message)
    {
        var json = JsonSerializer.Serialize(message, SharedJson.Options);
        return _writer.WriteLineAsync(json);
    }
}