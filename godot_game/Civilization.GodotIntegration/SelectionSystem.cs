using System.Linq;
using Civilization.Core.Game;
using Civilization.Core.Units;
using Civilization.GodotIntegration.Utils;
using Godot;

namespace Civilization.GodotIntegration;

public partial class SelectionSystem : Node2D
{
    public Unit? SelectedUnit { get; private set; }
    
    [Export] public SelectedUnitPanel UnitPanel;

    public bool TrySelectUnitAt(Vector2I tilePos, GameState state)
    {
        var coreTilePos = tilePos.ToCore();
        var unit = state.GetUnitsForPlayer(state.CurrentPlayer.Id).FirstOrDefault(u => u.Position == coreTilePos);
        if (unit == null) return false;
        
        SelectedUnit = unit;
        GD.Print($"Selected unit {unit.Id} at {tilePos} ({unit.Type})");
        
        UnitPanel.ShowFor(unit);
        return true;
    }
    
    public void Deselect()
    {
        SelectedUnit = null;
        UnitPanel.HidePanel();
    }
}