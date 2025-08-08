using Civilization.Core;

namespace Civilization.Shared.Commands;

public record MoveUnitCommand(Guid UnitId, Vec2I TargetPosition) : BaseCommand;