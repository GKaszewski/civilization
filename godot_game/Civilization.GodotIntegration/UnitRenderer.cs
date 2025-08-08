using System;
using System.Collections.Generic;
using Civilization.Core.Game;
using Godot;

namespace Civilization.GodotIntegration;

public partial class UnitRenderer : Node2D
{
    [Export] public PackedScene UnitScene;
    [Export] public MapRenderer MapRenderer;
    
    private readonly Dictionary<Guid, Node2D> _unitViews = new();

    public void Render(GameState state)
    {
        foreach (var view in _unitViews.Values) view.QueueFree();
        _unitViews.Clear();
        
        foreach (var unit in state.Units)
        {
            var unitNode = UnitScene.Instantiate<Node2D>();
            unitNode.Position = MapRenderer.MapToWorld(unit.Position);
            AddChild(unitNode);
            _unitViews[unit.Id] = unitNode;
        }
    }
}