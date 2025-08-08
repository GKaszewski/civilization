using Civilization.Core.Interfaces;

namespace Civilization.Core.Actions;

public class ExecutedAction
{
    public IGameAction Action { get; }
    public GameActionContext ContextSnapshot { get; }

    public ExecutedAction(IGameAction action, GameActionContext snapshot)
    {
        Action = action;
        ContextSnapshot = snapshot;
    }
}