using Civilization.Core.Game;

namespace Civilization.Core.Actions;

public class GameActionContext
{
    public GameMap Map { get; }
    public List<Player> Players { get; }
    public GameState State { get; }

    public GameActionContext(GameState gameState)
    {
        State = gameState;
        Map = gameState.Map;
        Players = gameState.Players;
    }
    
    public Player CurrentPlayer => State.CurrentPlayer;
}