

namespace Civilization.Core.Game;

public class Player(int id, string name, ColorRGBA color, bool isHuman = true)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public ColorRGBA Color { get; } = color;
    public bool IsHuman { get; } = isHuman;
}