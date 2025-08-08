namespace Civilization.Shared.Packets.ServerMessages;

public record ErrorMessage(string Reason) : BaseServerMessage;