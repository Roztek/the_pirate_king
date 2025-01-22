using Godot;
using Godot.Collections;
using System;

public partial class SwordAbilityController : Node
{
    [Export] public PackedScene sword_ability_scene { get; set; }
    
    public int base_damage = 5;
    public float additional_damage_percent = 1;
    public double base_wait_time;


    public override void _Ready()
	{
        Timer ability_timer = GetNode<Timer>("Timer");
        ability_timer.Timeout += OnTimerTimeout;

        GameEvents game_events = GetNode<GameEvents>("/root/GameEvents");
        game_events.AbilityUpgradeAdded += OnAbilityUpgradeAdded;

        base_wait_time = ability_timer.WaitTime;
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
            // Instantiate the sword and check if it was successful
            if (sword_ability_scene.Instantiate() is not SwordAbility sword_instance)
                return;

            Node2D foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;
            foreground_layer.AddChild(sword_instance);

            sword_instance.hitbox_component.damage = base_damage * additional_damage_percent;

            sword_instance.GlobalPosition = closest_enemy.GlobalPosition;
            float random_angle = (float)(new Random().NextDouble() * Math.PI * 2);
            sword_instance.GlobalPosition += Vector2.Right.Rotated(random_angle);

            var enemy_direction = closest_enemy.GlobalPosition - sword_instance.GlobalPosition;
            sword_instance.Rotation = enemy_direction.Angle();
        }
    }


    public void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades)
    {
        Timer ability_timer = GetNode<Timer>("Timer");

        if (upgrade.id == "sword_speed")
        {
            Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
            int quantity = (int) upgrade_data["quantity"];

            float percent_reduction = quantity * 0.1f;

            ability_timer.WaitTime = base_wait_time * (1 - percent_reduction);
            ability_timer.Start();
        }
        else if (upgrade.id == "sword_damage")
        {
            Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
            int quantity = (int) upgrade_data["quantity"];

            additional_damage_percent = 1 + (quantity * 0.15f);
        }
    }
}
