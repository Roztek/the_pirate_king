using Godot;
using System;

public partial class AxeAbilityController : Node
{
    [Export] public PackedScene axe_ability_scene { get; set; }

    public float damage = 10;


    public override void _Ready()
	{
        Timer ability_timer = GetNode<Timer>("Timer");
        ability_timer.Timeout += OnTimerTimeout;
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
        axe_instance.hitbox_component.damage = damage;
    }
}
