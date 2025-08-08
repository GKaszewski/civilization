namespace Civilization.Core.Interfaces;

public interface IEntity
{
    Guid Id { get; }
    int OwnerId { get; }
}