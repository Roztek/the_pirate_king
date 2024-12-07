using Godot;
using System;

public partial class EnemyManager : Node
{
	[Export] public PackedScene basic_enemy_scene { get; set; }
	[Export] public ArenaTimeManager arena_time_manager { get; set; }

	const int SPAWN_RADIUS = 376;

	public Timer enemy_timer = null;

	public double base_spawn_time = 0;
 

	public override void _Ready()
	{
		enemy_timer = GetNode<Timer>("Timer");
        enemy_timer.Timeout += OnTimerTimeout;

		base_spawn_time = enemy_timer.WaitTime;

		arena_time_manager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;
	}


	public void OnTimerTimeout()
	{
		enemy_timer.Start();

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


	public void OnArenaDifficultyIncreased(int arena_difficulty)
	{
		double time_off = (0.1 / 12) * arena_difficulty;
		time_off = Math.Min(time_off, 0.7);
		GD.Print(time_off);

		enemy_timer.WaitTime = base_spawn_time - time_off;
	}
}
