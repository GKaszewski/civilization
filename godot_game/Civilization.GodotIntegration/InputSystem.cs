using System;
using Civilization.Core.Actions;
using Godot;

namespace Civilization.GodotIntegration;

public partial class InputSystem : Node
{
    [Export] public MapRenderer MapRenderer;
    [Export] public SelectionSystem SelectionSystem;
    [Export] public GameStateProvider StateProvider;

    public Action? OnStateChanged;

    public override void _Ready()
    {
        MapRenderer.TileClicked += HandleTileClick;
    }
    
    public override void _ExitTree()
    {
        MapRenderer.TileClicked -= HandleTileClick;
    }

    private void HandleTileClick(Vector2I position, bool isRightClick)
    {
        var state = StateProvider.GameState;
        var context = new GameActionContext(state);
        var selected = SelectionSystem.SelectedUnit;
        
        if (selected != null)
        {
            if (!isRightClick) return;
            var move = new MoveUnitAction(selected.Id, position);
            
            if (!move.CanExecute(context)) return;
            
            state.ActionQueue.Enqueue(move);
            state.ActionQueue.ExecuteAll(context);
            OnStateChanged?.Invoke();
            return;
        }
        
        if (SelectionSystem.TrySelectUnitAt(position, state))
        {
            GD.Print($"InputSystem: unit {selected?.Id} selected at {position}");
        }
    }
}