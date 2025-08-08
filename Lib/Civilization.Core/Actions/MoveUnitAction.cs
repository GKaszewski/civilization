using Civilization.Core.Interfaces;


namespace Civilization.Core.Actions;

public class MoveUnitAction(Guid unitId, Vec2I targetPosition) : IGameAction
{
    public Guid UnitId { get; } = unitId;
    public Vec2I TargetPosition { get; } = targetPosition;

    public bool CanExecute(GameActionContext context)
    {
        var unit = context.State.FindUnit(UnitId);
        if (unit == null || unit.OwnerId != context.CurrentPlayer.Id) return false;
        
        return context.Map.Grid.IsValidPosition(TargetPosition) && unit.CanMoveTo(TargetPosition, context.Map);
    }

    public void Execute(GameActionContext context)
    {
        var unit = context.State.FindUnit(UnitId);
        if (unit == null) return;
        unit.Position = TargetPosition;
        unit.ActionPoints -= 1;
    }

    public void Undo(GameActionContext context)
    {
        
    }
}