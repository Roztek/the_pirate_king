using Godot;
using System;
using Godot.Collections;

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


	public Vector2 GetSpawnPosition()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player == null)
            return Vector2.Zero;

		Vector2 spawn_position = Vector2.Zero;
		float random_direction = (float)(new Random().NextDouble() * Math.PI * 2); 

		for(int i = 0; i < 4; i++)
		{
			Vector2 offset = new Vector2(
				Mathf.Cos(random_direction),
				Mathf.Sin(random_direction)
			) * SPAWN_RADIUS;		

			spawn_position = player.GlobalPosition + offset;

			var query_paramaters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawn_position, 1);
			Dictionary result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(query_paramaters);
		
			if (result.Count == 0)
			{
				break;
			}
			else
			{
				random_direction += 90;
			}
		}

		return spawn_position;
	}


	public void OnTimerTimeout()
	{
		enemy_timer.Start();

		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player == null)
            return;

		// Instantiate the enemy and check if it was successful
		if (basic_enemy_scene.Instantiate() is not Node2D enemy_instance)
			return;

		Node2D entities_layer = GetTree().GetFirstNodeInGroup("entities_layer") as Node2D;
		entities_layer.AddChild(enemy_instance);
		enemy_instance.GlobalPosition = GetSpawnPosition();
	}


	public void OnArenaDifficultyIncreased(int arena_difficulty)
	{
		double time_off = (0.1 / 12) * arena_difficulty;
		time_off = Math.Min(time_off, 0.7);
		GD.Print(time_off);

		enemy_timer.WaitTime = base_spawn_time - time_off;
	}
}
