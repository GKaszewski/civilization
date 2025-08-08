using Civilization.Core.Actions;

namespace Civilization.Core.Interfaces;

public interface IGameAction
{
    bool CanExecute(GameActionContext context);
    void Execute(GameActionContext context);
    void Undo(GameActionContext context);
}