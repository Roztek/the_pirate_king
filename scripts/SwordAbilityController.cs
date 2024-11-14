using Godot;
using Godot.Collections;
using System;

public partial class SwordAbilityController : Node
{
    const int MAX_RANGE = 150;

    [Export] public PackedScene sword_ability_scene { get; set; }

    private float damage = 5;

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

        var enemies = GetTree().GetNodesInGroup("enemy");
        if (enemies == null)
            return;

        // Find the closest enemy by comparing distances to the player
        Node2D closest_enemy = null;
        float closest_distance = float.MaxValue;

        foreach (Node enemy_node in enemies)
        {
            Node2D enemy = (Node2D)enemy_node;

            float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
            if (distance < closest_distance)
            {
                closest_distance = distance;
                closest_enemy = enemy;
            }
        }

        // Check if we found a closest enemy, then spawn the sword at that position
        if (closest_enemy != null)
        {
            var sword_instance = sword_ability_scene.Instantiate() as SwordAbility;
            if (sword_instance == null)
                return;

            sword_instance.hitbox_component.damage = damage;

            sword_instance.GlobalPosition = closest_enemy.GlobalPosition;
            GetTree().Root.AddChild(sword_instance);

            float randomAngle = (float)(new Random().NextDouble() * Math.PI * 2);
            sword_instance.GlobalPosition += Vector2.Right.Rotated(randomAngle);

            var enemy_direction = closest_enemy.GlobalPosition - sword_instance.GlobalPosition;
            sword_instance.Rotation = enemy_direction.Angle();
        }
    }
}
