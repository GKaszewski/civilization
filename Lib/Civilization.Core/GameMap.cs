using Civilization.Core.Interfaces;


namespace Civilization.Core;

public class GameMap
{
    private readonly Dictionary<Vec2I, Tile.Tile> _tiles = new();

    public ITileGrid Grid { get; private set; }
    
    public GameMap(ITileGrid grid)
    {
        Grid = grid;
        
        for (var x = 0; x < ((IGridSize)grid).Width; x++)
        for (var y = 0; y < ((IGridSize)grid).Height; y++)
        {
            var pos = new Vec2I(x, y);
            _tiles[pos] = new Tile.Tile(pos);
        }
    }

    
    public IEnumerable<Tile.Tile> GetTiles() => _tiles.Values;
    
    public IEnumerable<Tile.Tile> GetNeighbors(Vec2I position)
    {
       return Grid.GetNeighbors(position).Select(GetTile).Where(t => t != null)!;
    }
    
    public Tile.Tile? GetTile(Vec2I position) => _tiles.GetValueOrDefault(position);
}