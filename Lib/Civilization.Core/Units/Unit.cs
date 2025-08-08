using Civilization.Core.Game;
using Civilization.Core.Interfaces;


namespace Civilization.Core.Units;

public class Unit : IOnTurnListener, IEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public int OwnerId { get; }
    public UnitType Type { get; }
    public UnitData Data { get; }

    public Vec2I Position { get; set; }
    public int MaxActionPoints { get; }
    public int ActionPoints { get; set; }

    public Unit(int ownerId, UnitType type, Vec2I position)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Type = type;
        Position = position;
        
        Data = UnitDataRegistry.Get(Type);
        MaxActionPoints = Data.MaxActionPoints;
        ActionPoints = MaxActionPoints;
    }

    public void ResetActionPoints()
    {
        ActionPoints = MaxActionPoints;
    }

    public bool CanMoveTo(Vec2I destination, GameMap map)
    {
        if (ActionPoints <= 0) return false;
        
        if (!map.Grid.IsValidPosition(destination)) return false;
        
        var distance = Math.Abs(Position.X - destination.X) + Math.Abs(Position.Y - destination.Y);
        return distance <= Data.MoveRange;
    }
    
    public bool HasTag(UnitTag tag) => Data.HasTag(tag);

    public void OnTurnStart(GameState state)
    {
        ResetActionPoints();
    }

    public void OnTurnEnd(GameState state)
    {
        
    }
}