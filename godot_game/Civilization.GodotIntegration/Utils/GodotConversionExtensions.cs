using Civilization.Core;
using Godot;

namespace Civilization.GodotIntegration.Utils;

public static class GodotConversionExtensions
{
    public static Vector2I ToGodot(this Vec2I v) => new(v.X, v.Y);
    public static Vec2I ToCore(this Vector2I v) => new(v.X, v.Y);
    
    public static Color ToGodot(this ColorRGBA c) => new(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
    public static ColorRGBA ToCore(this Color c) => new((byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255), (byte)(c.A * 255));
}