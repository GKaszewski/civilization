using Civilization.Core.Interfaces;

namespace Civilization.Core.Actions;

public class ActionQueue
{
    private readonly Queue<IGameAction> _pending = new();
    private readonly Stack<ExecutedAction> _history = new();

    public void Enqueue(IGameAction action)
    {
        _pending.Enqueue(action);
    }

    public void ExecuteAll(GameActionContext context)
    {
        while (_pending.Count > 0)
        {
            var action = _pending.Dequeue();
            if (!action.CanExecute(context)) continue;
            
            action.Execute(context);
            _history.Push(new ExecutedAction(action, context));
        }
    }
}