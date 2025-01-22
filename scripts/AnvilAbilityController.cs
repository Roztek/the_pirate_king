using Godot;
using System;
using Godot.Collections;

public partial class AnvilAbilityController : Node
{
    [Export] public PackedScene anvil_ability_scene { get; set; }

    public const int BASE_RANGE = 100;

    public Timer ability_timer = null;
    public float base_damage = 15;
    public float additional_damage_percent = 1;
    public int anvil_amount = 0;


    public override void _Ready()
	{
        ability_timer = GetNode<Timer>("Timer");
        ability_timer.Timeout += OnTimerTimeout;

        GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}


    public void OnTimerTimeout()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        Vector2 direction = Vector2.Right.Rotated((float) GD.RandRange(0, Math.PI * 2));
        float additional_rotation_degrees = 360.0f / (anvil_amount + 1);
        float anvil_distance = GD.RandRange(0, BASE_RANGE);

        for (int i = 0; i < anvil_amount + 1; i++)
        {
            Vector2 adjusted_direction = direction.Rotated(Mathf.DegToRad(i * additional_rotation_degrees));
            Vector2 spawn_position = player.GlobalPosition + (adjusted_direction * anvil_distance);

            var query_parameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawn_position, 1);
            Dictionary result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(query_parameters);

            if (result.Count > 0)
                spawn_position = (Vector2) result["position"];

            if (anvil_ability_scene.Instantiate() is not AnvilAbility anvil_instance)
                continue;

            Node2D foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;
            if (foreground_layer == null)
                continue;

            foreground_layer.AddChild(anvil_instance);
            anvil_instance.GlobalPosition = spawn_position;
            anvil_instance.hitbox_component.damage = base_damage * additional_damage_percent;
        }
    }


    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades)
    {
        if (upgrade.id == "anvil_count")
            anvil_amount = (int) current_upgrades["anvil_count"]["quantity"];
    }
}
