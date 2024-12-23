using Godot;
using System;
using Godot.Collections;

public partial class UpgradeScreen : CanvasLayer
{
    [Signal] public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

    [Export] public PackedScene upgrade_card_scene { get; set; }

    private HBoxContainer _card_container = null;


    public override void _Ready()
    {
        GetTree().Paused = true;
    }


    public void SetAbilityUpgrades(Array<AbilityUpgrade> upgrades)
    {
        float delay = 0;
        _card_container = GetNode<HBoxContainer>("%CardContainer");

        foreach (AbilityUpgrade upgrade in upgrades) 
        {
            var card_instance = upgrade_card_scene.Instantiate() as AbilityUpgradeCard;
            _card_container.AddChild(card_instance);
            card_instance.SetAbilityUpgrade(upgrade);
            card_instance.PlayIn(delay);
            card_instance.Selected += () => OnUpgradeSelectedUi(upgrade);
            delay += 0.2f;
        }
    }


    public void OnUpgradeSelectedUi(AbilityUpgrade upgrade)
    {
        EmitSignal(SignalName.UpgradeSelected, upgrade);
        GetTree().Paused = false;
        QueueFree();
    }
}
