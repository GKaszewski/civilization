using Civilization.Shared.Commands;

namespace Civilization.Shared.Packets;

public record ClientMessage(int PlayerId, BaseCommand Command);