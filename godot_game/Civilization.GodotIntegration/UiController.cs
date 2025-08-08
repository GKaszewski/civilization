using Civilization.Core.Actions;
using Civilization.Core.Game;
using Godot;

namespace Civilization.GodotIntegration;

public partial class UiController : Node
{
    [Export] public Label TurnLabel;
    [Export] public SelectedUnitPanel UnitPanel;
    [Export] public GameStateProvider StateProvider;
    [Export] public SelectionSystem SelectionSystem;
    [Export] public GameController GameController;

    public override void _Ready()
    {
        UnitPanel.OnSettleClicked = TrySettleCity;
    }
    
    public override void _Process(double delta)
    {
        TurnLabel.Text = $"Turn: {StateProvider.GameState.TurnManager.TurnNumber} - {StateProvider.GameState.CurrentPlayer.Name}";
    }
    
    private void TrySettleCity()
    {
        var selected = SelectionSystem.SelectedUnit;
        if (selected == null) return;
        
        var context = new GameActionContext(StateProvider.GameState);
        var settle = new SettleCityAction(selected.Id);
        if (!settle.CanExecute(context))
        {
            GD.PrintErr($"Cannot settle city with unit {selected.Id} at {selected.Position}");
            return;
        }
        
        StateProvider.GameState.ActionQueue.Enqueue(settle);
        StateProvider.GameState.ActionQueue.ExecuteAll(context);
        SelectionSystem.Deselect();
        GameController.Redraw();
    }
}