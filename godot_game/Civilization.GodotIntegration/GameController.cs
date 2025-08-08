using System.Collections.Generic;
using Civilization.Core;
using Civilization.Core.Game;
using Civilization.Core.Grid;
using Civilization.Core.Units;
using Godot;

namespace Civilization.GodotIntegration;

public partial class GameController : Node
{
    private const int DefaultWorldSize = 10;

    [Export] public GameStateProvider StateProvider;
    [Export] public MapRenderer MapRenderer;
    [Export] public UnitRenderer UnitRenderer;
    [Export] public CityRenderer CityRenderer;
    [Export] public InputSystem InputSystem;
    [Export] public SelectionSystem SelectionSystem;

    public override void _Ready()
    {
        // Setup initial game state
        var grid = new SquareGrid(DefaultWorldSize, DefaultWorldSize);
        var gameMap = new GameMap(grid);

        var players = new List<Player>
        {
            new Player(0, "Player 1", Colors.Red),
            new Player(1, "Player 2", Colors.Blue)
        };

        var gameState = new GameState(gameMap, players);
        StateProvider.Initialize(gameState);

        // Setup UI systems
        MapRenderer.RenderMap(gameMap);
        InputSystem.OnStateChanged = Redraw;

        // Add one settler to start
        var settler = new Unit(0, UnitType.Settler, new Vector2I(2, 2));
        gameState.AddUnit(settler);
        GD.Print($"Added settler unit at {settler.Position}");

        Redraw();
        GD.Print($"Turn {gameState.TurnManager.TurnNumber}: {gameState.CurrentPlayer.Name}'s turn");
    }
    
    public void OnEndTurnPressed()
    {
        StateProvider.GameState.NextTurn();
        Redraw();
        GD.Print($"Turn {StateProvider.GameState.TurnManager.TurnNumber}: {StateProvider.GameState.CurrentPlayer.Name}'s turn");
    }

    public void Redraw()
    {
        UnitRenderer.Render(StateProvider.GameState);
        CityRenderer.Render(StateProvider.GameState);
    }
}