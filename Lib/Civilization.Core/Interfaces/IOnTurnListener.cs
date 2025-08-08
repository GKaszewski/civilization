using Civilization.Core.Game;

namespace Civilization.Core.Interfaces;

public interface IOnTurnListener
{
    void OnTurnStart(GameState state);
    void OnTurnEnd(GameState state);
}