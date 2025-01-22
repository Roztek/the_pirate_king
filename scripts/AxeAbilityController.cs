using Godot;
using Godot.Collections;

public partial class AxeAbilityController : Node
{
    [Export] public PackedScene axe_ability_scene { get; set; }

    public float base_damage = 10;
    public float additional_damage_percent = 1;


    public override void _Ready()
	{
        Timer ability_timer = GetNode<Timer>("Timer");
        ability_timer.Timeout += OnTimerTimeout;

        GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
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


    public void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades)
    {
        if (upgrade.id == "axe_damage")
            additional_damage_percent = 1 + ((int) current_upgrades["axe_damage"]["quantity"] * 0.1f);
    }
}
