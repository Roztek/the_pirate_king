using Godot;
using Godot.Collections;
using System;

public partial class AxeAbilityController : Node
{
    [Export] public PackedScene axe_ability_scene { get; set; }

    public float base_damage = 10;
    public float additional_damage_percent = 1;


    public override void _Ready()
	{
        Timer ability_timer = GetNode<Timer>("Timer");
        ability_timer.Timeout += OnTimerTimeout;

        GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
        game_events.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}


    public void OnTimerTimeout()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        Node2D foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;
        if (foreground_layer == null)
            return;

        // Instantiate the sword and check if it was successful
        if (axe_ability_scene.Instantiate() is not AxeAbility axe_instance)
            return;

        foreground_layer.AddChild(axe_instance);
        axe_instance.GlobalPosition = player.GlobalPosition;
        axe_instance.hitbox_component.damage = base_damage * additional_damage_percent;
    }


    public void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary current_upgrades)
    {
        if (upgrade.id == "axe_damage")
        {
            Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
            int quantity = (int) upgrade_data["quantity"];

            additional_damage_percent = 1 + (quantity * 0.1f);
        }
    }
}
