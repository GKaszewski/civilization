namespace Civilization.Core;

public readonly struct Vec2I(int x, int y) : IEquatable<Vec2I>
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public static Vec2I operator +(Vec2I a, Vec2I b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2I operator -(Vec2I a, Vec2I b) => new(a.X - b.X, a.Y - b.Y);

    public override string ToString() => $"({X}, {Y})";
    public bool Equals(Vec2I other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Vec2I other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}