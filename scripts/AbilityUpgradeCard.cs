using Godot;
using System;

public partial class AbilityUpgradeCard : PanelContainer
{
    [Signal] public delegate void SelectedEventHandler();

    private Label _name_label = null;
    private Label _description_label = null;
    
    
    public override void _Ready()
    {
        _name_label = GetNode<Label>("%NameLabel");
        _description_label = GetNode<Label>("%DescriptionLabel");

        GuiInput += OnGuiInput;
    }


    public void SetAbilityUpgrade(AbilityUpgrade upgrade)
    {
        _name_label.Text = upgrade.name;
        _description_label.Text = upgrade.description;
    }


    public void OnGuiInput(InputEvent input)
    {
        if (input.IsActionPressed("left_click"))
            EmitSignal(SignalName.Selected);
    }
}
