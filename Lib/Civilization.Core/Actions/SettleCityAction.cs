using Civilization.Core.Game;
using Civilization.Core.Interfaces;
using Civilization.Core.Units;

namespace Civilization.Core.Actions;

public class SettleCityAction(Guid unitId) : IGameAction
{
    private const int CityNearbyRange = 4;
    public Guid UnitId { get; } = unitId;

    public bool CanExecute(GameActionContext context)
    {
        var unit = context.State.FindUnit(UnitId);
        
        if (unit == null || !unit.HasTag(UnitTag.Settle)) return false;
        if (unit.OwnerId != context.CurrentPlayer.Id) return false;
        if (unit.ActionPoints < 1) return false;

        var tile = context.Map.GetTile(unit.Position);
        if (tile is not { OwnerId: null }) return false;

        // Later we could also check if there is city nearby
        
        return true;
    }

    public void Execute(GameActionContext context)
    {
        var unit = context.State.FindUnit(UnitId)!;
        var position = unit.Position;

        context.State.RemoveUnit(UnitId);
        
        var city = new City(unit.OwnerId, position);
        city.ClaimTile(context.Map, position);
        
        context.State.AddCity(city);
    }

    public void Undo(GameActionContext context)
    {
        
    }
}