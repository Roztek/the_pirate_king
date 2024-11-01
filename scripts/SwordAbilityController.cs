using Godot;
using Godot.Collections;
using System;

public partial class SwordAbilityController : Node
{
    const int MAX_RANGE = 150;

    [Export]
    public PackedScene SwordAbilityScene { get; set; }


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
        Node2D closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Node enemy_node in enemies)
        {
            Node2D enemy = (Node2D)enemy_node;

            float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        // Check if we found a closest enemy, then spawn the sword at that position
        if (closestEnemy != null)
        {
            var sword_instance = SwordAbilityScene.Instantiate() as Node2D;
            if (sword_instance != null)
            {
                sword_instance.GlobalPosition = closestEnemy.GlobalPosition;
                GetTree().Root.AddChild(sword_instance);

                float randomAngle = (float)(new Random().NextDouble() * Math.PI * 2);
                sword_instance.GlobalPosition += Vector2.Right.Rotated(randomAngle);

                var enemy_direction = closestEnemy.GlobalPosition - sword_instance.GlobalPosition;
                sword_instance.Rotation = enemy_direction.Angle();
            }
        }
    }
}
