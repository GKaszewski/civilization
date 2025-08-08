namespace Civilization.Core.Units;

public class UnitData
{
    public string Name { get; init; }
    public int MaxActionPoints { get; init; }
    public int MoveRange { get; init; } = 1;
    public HashSet<UnitTag> Tags { get; init; } = [];
    
    public bool HasTag(UnitTag tag) => Tags.Contains(tag);
}