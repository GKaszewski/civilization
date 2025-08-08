using Civilization.Core.Actions;
using Civilization.Core.Interfaces;


namespace Civilization.Core.Game;

public class City : IOnTurnListener, IEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public int OwnerId { get; }
    public string Name { get; set; }
    
    public Vec2I Position { get; }
    public HashSet<Vec2I> Territory { get; } = [];
    
    public int MaxActionPoints { get; private set; } = 1;
    public int ActionPoints { get; set; }
    
    public City(int ownerId, Vec2I position, string name = "New City")
    {
        OwnerId = ownerId;
        Position = position;
        Name = name;
        Territory.Add(position);
        ActionPoints = MaxActionPoints;
    }

    public void ExpandTo(Vec2I newTile) => Territory.Add(newTile);

    public void ResetActionPoints() => ActionPoints = MaxActionPoints;

    public bool CanProduce() => ActionPoints > 0;

    public void SpendActionPoint()
    {
        if (ActionPoints > 0) ActionPoints--;
    }

    public void ClaimTile(GameMap map, Vec2I tilePos)
    {
        var tile = map.GetTile(tilePos);
        if (tile == null) return;
        
        tile.OwnerId = OwnerId;
        Territory.Add(tilePos);
    }
    

    public void OnTurnStart(GameState state)
    {
        ResetActionPoints();
        
        var map = state.Map;
        var neighbors = Territory.SelectMany(pos => map.Grid.GetNeighbors(pos))
            .Distinct()
            .Where(p => map.GetTile(p)?.OwnerId == null)
            .ToList();

        if (neighbors.Count > 0)
        {
            var rng = new Random();
            var choice = neighbors[rng.Next(neighbors.Count)];
            state.ActionQueue.Enqueue(new ExpandTerritoryAction(Id, choice));
        }
    }

    public void OnTurnEnd(GameState state)
    {
        
    }
}