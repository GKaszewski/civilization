using Civilization.Core.Game;

namespace Civilization.Shared.Packets.ServerMessages;

public record StateUpdateMessage(GameState GameState, PlayerInfo CurrentPlayer) : BaseServerMessage;