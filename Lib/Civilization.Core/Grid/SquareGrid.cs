using Civilization.Core.Interfaces;


namespace Civilization.Core.Grid;

public class SquareGrid : ITileGrid, IGridSize
{
    public int Width { get; }
    public int Height { get; }
    
    private static readonly Vec2I[] NeighborOffsets = 
    {
        new Vec2I(-1, 0), // Left
        new Vec2I(1, 0),  // Right
        new Vec2I(0, -1), // Up
        new Vec2I(0, 1)   // Down
    };
    
    public SquareGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }
    
    public IEnumerable<Vec2I> GetNeighbors(Vec2I tilePosition)
    {
        foreach (var offset in NeighborOffsets)
        {
            var neighbor = tilePosition + offset;
            if (IsValidPosition(neighbor))
                yield return neighbor;
        }
    }

    public Vec2I ClampPosition(Vec2I position)
    {
        var x = Math.Clamp(position.X, 0, Width - 1);
        var y = Math.Clamp(position.Y, 0, Height - 1);
        return new Vec2I(x, y);
    }

    public bool IsValidPosition(Vec2I position)
    {
        return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
    }
}