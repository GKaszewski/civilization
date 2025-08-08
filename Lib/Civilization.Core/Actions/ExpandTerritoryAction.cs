using Civilization.Core.Interfaces;


namespace Civilization.Core.Actions;

public class ExpandTerritoryAction(Guid cityId, Vec2I targetTile) : IGameAction
{
    public Guid CityId { get; } = cityId;
    public Vec2I TargetTile { get; } = targetTile;
    
    public bool CanExecute(GameActionContext context)
    {
        var city = context.State.FindCity(CityId);
        if (city == null || city.ActionPoints < 1) return false;
        
        if (!context.Map.Grid.IsValidPosition(TargetTile)) return false;
        
        var tile = context.Map.GetTile(TargetTile);
        if (tile is not { OwnerId: null }) return false;

        return city.Territory.Any(t =>
        {
            var neighbors = context.Map.GetNeighbors(t);
            return neighbors.Contains(tile);
        });
    }

    public void Execute(GameActionContext context)
    {
        var city = context.State.FindCity(CityId)!;
        city.ClaimTile(context.Map, TargetTile);
        city.SpendActionPoint();
    }

    public void Undo(GameActionContext context)
    {
        
    }
}