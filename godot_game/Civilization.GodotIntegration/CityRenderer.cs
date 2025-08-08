using System;
using System.Collections.Generic;
using Civilization.Core.Game;
using Godot;

namespace Civilization.GodotIntegration;

public partial class CityRenderer : Node2D
{
    [Export] public PackedScene CityScene;
    [Export] public MapRenderer MapRenderer;
    
    private readonly Dictionary<Guid, Node2D> _cityViews = new();

    public void Render(GameState state)
    {
        foreach (var view in _cityViews.Values) view.QueueFree();
        _cityViews.Clear();
        
        foreach (var city in state.Cities)
        {
            var cityNode = CityScene.Instantiate<Node2D>();
            cityNode.Position = MapRenderer.MapToWorld(city.Position);
            AddChild(cityNode);
            _cityViews[city.Id] = cityNode;
        }
    }
}