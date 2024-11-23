using Godot;
using System;

public partial class EnemyManager : Node
{
	[Export] public PackedScene basic_enemy_scene { get; set; }

	const int SPAWN_RADIUS = 376;
 

	public override void _Ready()
	{
		Timer enemy_timer = GetNode<Timer>("Timer");
        enemy_timer.Timeout += OnTimerTimeout;
	}


	public void OnTimerTimeout()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player == null)
            return;

		float random_direction = (float)(new Random().NextDouble() * Math.PI * 2); 

		Vector2 offset = new Vector2(
			Mathf.Cos(random_direction),
			Mathf.Sin(random_direction)
		) * SPAWN_RADIUS;		

		Vector2 spawn_position = player.GlobalPosition + offset;

		// Instantiate the enemy and check if it was successful
		if (basic_enemy_scene.Instantiate() is not Node2D enemy_instance)
			return;

		Node2D entities_layer = GetTree().GetFirstNodeInGroup("entities_layer") as Node2D;
		entities_layer.AddChild(enemy_instance);
		enemy_instance.GlobalPosition = spawn_position;
	}
}
