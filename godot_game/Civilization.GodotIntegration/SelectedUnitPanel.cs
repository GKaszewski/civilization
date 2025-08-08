using System;
using Civilization.Core.Units;
using Godot;

namespace Civilization.GodotIntegration;

public partial class SelectedUnitPanel : Control
{
    [Export] public Label UnitInfoLabel;
    [Export] public Button SettleButton;

    public Action? OnSettleClicked;

    public override void _Ready()
    {
        SettleButton.Pressed += () => OnSettleClicked?.Invoke();
        Hide();
    }

    public void ShowFor(Unit unit)
    {
        GD.Print($"Showing unit panel for {unit.Id} at {unit.Position} ({unit.Type})");
        UnitInfoLabel.Text = $"{unit.Type} at {unit.Position} ({unit.ActionPoints} AP)";
        Show();
    }

    public void HidePanel() => Hide();
}