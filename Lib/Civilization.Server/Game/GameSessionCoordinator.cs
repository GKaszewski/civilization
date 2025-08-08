using Civilization.Core.Interfaces;
using Civilization.Server.Networking.Interfaces;
using Civilization.Shared.Commands;
using Civilization.Shared.Packets;
using Civilization.Shared.Packets.ServerMessages;

namespace Civilization.Server.Game;

public class GameSessionCoordinator
{
    private readonly GameSession _session;
    private readonly Dictionary<int, bool> _connectedPlayers = new();
    private readonly Dictionary<int, IClientConnection> _connections = new();

    public GameSessionCoordinator()
    {
        _session = new GameSession(this);
    }

    public Task RegisterPlayerAsync(int playerId, IClientConnection connection)
    {
        _connectedPlayers[playerId] = true;
        _connections[playerId] = connection;

        Console.WriteLine($"Player {playerId} registered.");
    
        // Send initial log message
        return connection.SendAsync(new LogMessage("Welcome to the game!"));
    }
    
    public async Task ReceiveCommandAsync(ClientMessage message)
    {
        if (!_connectedPlayers.ContainsKey(message.PlayerId))
        {
            Console.WriteLine($"Rejected command from unregistered player {message.PlayerId}.");
            return;
        }

        switch (message.Command)
        {
            case EndTurnCommand:
                await HandleEndTurnAsync(message.PlayerId);
                break;

            default:
                try
                {
                    await _session.ProcessCommand(message);
                    var state = _session.State;
                    var currentPlayerInfo = new PlayerInfo(state.CurrentPlayer.Id, state.CurrentPlayer.Name, state.CurrentPlayer.Color.ToHex());
                    await BroadcastAsync(new StateUpdateMessage(_session.State, currentPlayerInfo));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Player {message.PlayerId}] Error: {ex.Message}");
                    await SendToPlayerAsync(message.PlayerId, new ErrorMessage(ex.Message));
                }
                break;
        }
    }
    
    public async Task SendToPlayerAsync(int playerId, BaseServerMessage message)
    {
        if (_connections.TryGetValue(playerId, out var conn))
        {
            await conn.SendAsync(message);
        }
    }

    public async Task BroadcastAsync(BaseServerMessage message)
    {
        foreach (var conn in _connections.Values)
        {
            await conn.SendAsync(message);
        }
    }
    
    public async Task BroadcastStateUpdateAsync()
    {
        var state = _session.State;
        var currentPlayerInfo = new PlayerInfo(state.CurrentPlayer.Id, state.CurrentPlayer.Name, state.CurrentPlayer.Color.ToHex());
        var msg = new StateUpdateMessage(state, currentPlayerInfo);
        await BroadcastAsync(msg);
    }
    
    public async Task OnTurnAdvancedAsync()
    {
        var state = _session.State;
        var currentPlayerInfo = new PlayerInfo(state.CurrentPlayer.Id, state.CurrentPlayer.Name, state.CurrentPlayer.Color.ToHex());
        var message = new StateUpdateMessage(_session.State, currentPlayerInfo);
        await BroadcastAsync(message);
    }
    
    public async Task HandleEndTurnAsync(int playerId)
    {
        if (playerId != _session.State.CurrentPlayer.Id)
        {
            Console.WriteLine($"[Turn] Player {playerId} attempted to end turn out of order.");
            await SendToPlayerAsync(playerId, new ErrorMessage("Not your turn."));
            return;
        }

        _session.State.NextTurn();

        Console.WriteLine($"[Turn] Player {playerId} ended their turn. Now it's Player {_session.State.CurrentPlayer.Id}'s turn.");

        await BroadcastAsync(new LogMessage($"Player {playerId} ended their turn."));
        await OnTurnAdvancedAsync();
    }
    
    // Later: Add methods like:
    // - BroadcastGameState()
    // - GetStateForPlayer(int playerId)
    // - NotifyTurnAdvance()
}