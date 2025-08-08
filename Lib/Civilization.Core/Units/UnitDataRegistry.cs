namespace Civilization.Core.Units;

public class UnitDataRegistry
{
    private static readonly Dictionary<UnitType, UnitData> _registry = new()
    {
        [UnitType.Settler] = new UnitData
        {
            Name = "Settler",
            MaxActionPoints = 1,
            MoveRange = 1,
            Tags = new() { UnitTag.Settle, }
        }
    };
    
    public static UnitData Get(UnitType unitType) => _registry[unitType];
}