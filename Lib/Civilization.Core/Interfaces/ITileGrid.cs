

namespace Civilization.Core.Interfaces;

public interface ITileGrid
{
    IEnumerable<Vec2I> GetNeighbors(Vec2I tilePosition);
    Vec2I ClampPosition(Vec2I position);
    bool IsValidPosition(Vec2I position);
}