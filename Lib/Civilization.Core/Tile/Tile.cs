

namespace Civilization.Core.Tile;

public class Tile
{
    public Vec2I Position { get; }
    public TileType Type { get; set; } = TileType.Plain;
    public int? OwnerId { get; set; } = null;
    
    public Tile(Vec2I position) 
    {
        Position = position;
    }
    
}