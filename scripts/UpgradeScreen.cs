using Godot;
using System;
using Godot.Collections;

public partial class UpgradeScreen : CanvasLayer
{
    [Export] public PackedScene upgrade_card_scene { get; set; }

    private HBoxContainer _card_container = null;


    public override void _Ready()
    {
        GetTree().Paused = true;
    }


    public void SetAbilityUpgrades(Array<AbilityUpgrade> upgrades)
    {
        _card_container = GetNode<HBoxContainer>("%CardContainer");

        foreach (AbilityUpgrade upgrade in upgrades) 
        {
            var card_instance = upgrade_card_scene.Instantiate() as AbilityUpgradeCard;
            _card_container.AddChild(card_instance);
            card_instance.SetAbilityUpgrade(upgrade);
        }
    }
}
