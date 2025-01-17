using Godot;
using System;
using Godot.Collections;

public partial class AnvilAbilityController : Node
{
    [Export] public PackedScene anvil_ability_scene { get; set; }

    public const int BASE_RANGE = 100;

    public int base_damage = 15;


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

        Vector2 direction = Vector2.Right.Rotated((float) (new Random().NextDouble() * Math.PI * 2));
        Vector2 spawn_position = player.GlobalPosition + (direction * GD.RandRange(0, BASE_RANGE));

        var query_paramaters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawn_position, 1);
		Dictionary result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(query_paramaters);

        if (!(result.Count == 0))
            spawn_position = (Vector2) result["position"];

        if (anvil_ability_scene.Instantiate() is not AnvilAbility anvil_instance)
                return;

        Node2D foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;
        foreground_layer.AddChild(anvil_instance);
        anvil_instance.GlobalPosition = spawn_position;
        anvil_instance.hitbox_component.damage = base_damage;
    }
}
