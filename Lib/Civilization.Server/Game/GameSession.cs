using Civilization.Core;
using Civilization.Core.Actions;
using Civilization.Core.Game;
using Civilization.Core.Grid;
using Civilization.Core.Interfaces;
using Civilization.Core.Units;
using Civilization.Shared.Commands;
using Civilization.Shared.Packets;
using Civilization.Shared.Packets.ServerMessages;

namespace Civilization.Server.Game;

public class GameSession
{
    private const int DefaultSize = 10;
    private readonly GameState _state;
    private readonly GameSessionCoordinator _coordinator;
    
    public GameState State => _state;

    public GameSession(GameSessionCoordinator coordinator)
    {
        _coordinator = coordinator;
        var grid = new SquareGrid(DefaultSize, DefaultSize);
        var map = new GameMap(grid);
        
        var players = new List<Player>
        {
            new(0, "Player 1", new ColorRGBA(255, 0, 0)),
            new(1, "Player 2", new ColorRGBA(0, 0, 255)),
        };
        
        _state = new GameState(map, players);
        
        _state.AddUnit(new Unit(0, UnitType.Settler, new Vec2I(2, 2)));
    }

    public Task ProcessCommand(ClientMessage msg)
    {
        var context = new GameActionContext(_state);

        switch (msg.Command)
        {
            case MoveUnitCommand move:
                EnqueueAndExecute(new MoveUnitAction(move.UnitId, move.TargetPosition), context);
                break;
            case SettleCityCommand settle:
                EnqueueAndExecute(new SettleCityAction(settle.UnitId), context);
                break;
            default:
                throw new NotSupportedException($"Command type {msg.Command.GetType()} is not supported.");
        }

        return Task.CompletedTask;
    }

    private void EnqueueAndExecute(IGameAction action, GameActionContext context)
    {
        if (!action.CanExecute(context))
        {
            Console.WriteLine($"Rejected invalid command from player {context.CurrentPlayer.Id}");
            return;
        }

        _state.ActionQueue.Enqueue(action);
        _state.ActionQueue.ExecuteAll(context);
    }
}