using Civilization.Core.Game;
using Godot;

namespace Civilization.GodotIntegration;

public partial class GameStateProvider : Node
{
    public GameState GameState { get; private set; }
    
    public void Initialize(GameState gameState)
    {
        GameState = gameState;
        GD.Print("GameStateProvider initialized with game state.");
    }
}