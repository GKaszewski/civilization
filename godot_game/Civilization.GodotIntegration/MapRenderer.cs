using Civilization.Core;
using Godot;

namespace Civilization.GodotIntegration;

public partial class MapRenderer : Node2D
{
    private const int TileIndexOffset = 1;
    
    [Export] public TileMapLayer TileMapLayer;
    [Export] public TileSet TileSet;
    
    [Signal] public delegate void TileClickedEventHandler(Vector2I position, bool isRightClick);

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseButton { Pressed: true }) return;
        
        var worldPos = GetGlobalMousePosition();
        var localPos = TileMapLayer.ToLocal(worldPos);
        var tilePos = TileMapLayer.LocalToMap(localPos);
        EmitSignalTileClicked(tilePos, @event is InputEventMouseButton { ButtonIndex: MouseButton.Right });
    }

    public void RenderMap(GameMap map)
    {
        TileMapLayer.SetTileSet(TileSet);
        var tileSetSource = TileSet.GetSource(1);
        TileMapLayer.Clear();
        
        foreach (var tile in map.GetTiles())
        {
            var pos = tile.Position;
            var tileId = (int)tile.Type + TileIndexOffset;
            var atlasCoords = tileSetSource.GetTileId(tileId);
            TileMapLayer.SetCell(pos, tileId, atlasCoords);
        }
    }
    
    public Vector2 MapToWorld(Vector2I position) => TileMapLayer.MapToLocal(position);
}