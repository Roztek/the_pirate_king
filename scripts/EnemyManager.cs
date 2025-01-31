using Godot;
using System;

public partial class EnemyManager : Node
{
	[Export] public PackedScene basic_enemy_scene { get; set; }
	[Export] public PackedScene ghost_enemy_scene { get; set; }
	[Export] public PackedScene flying_enemy_scene { get; set; }
	[Export] public ArenaTimeManager arena_time_manager { get; set; }

	private const int SPAWN_RADIUS = 376;

	public Timer enemy_timer = null;
	public int number_to_spawn = 1;
	public double base_spawn_time = 0;
	public WeightedTable enemy_table = new WeightedTable();
 

	public override void _Ready()
	{
		enemy_table.AddItem(basic_enemy_scene, 20);

		enemy_timer = GetNode<Timer>("Timer");
        enemy_timer.Timeout += OnTimerTimeout;
		base_spawn_time = enemy_timer.WaitTime;

		arena_time_manager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;
	}


	private Vector2 GetSpawnPosition()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player == null)
			return Vector2.Zero;

		Vector2 spawn_position = Vector2.Zero;
		Vector2 random_direction = Vector2.Right.Rotated((float) GD.RandRange(0, Mathf.Tau));
		for (int i = 0; i < 4; i++)
		{
			spawn_position = player.GlobalPosition + (random_direction * SPAWN_RADIUS);
			var additional_offset = random_direction * 20;

			var query_parameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawn_position + additional_offset, 1 << 0);
			var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(query_parameters);
			if (result.Count <= 0)
				break;
			
			random_direction = random_direction.Rotated(Mathf.DegToRad(90));
		}

		return spawn_position;
	}


	public void OnTimerTimeout()
	{
		enemy_timer.Start();

		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player == null)
            return;

		for (int i = 0; i < number_to_spawn; i++)
		{
			var enemy_scene = enemy_table.PickRandom() as PackedScene;
			// Instantiate the enemy and check if it was successful
			if (enemy_scene.Instantiate() is not Node2D enemy_instance)
				return;

			Node2D entities_layer = GetTree().GetFirstNodeInGroup("entities_layer") as Node2D;
			entities_layer.AddChild(enemy_instance);
			enemy_instance.GlobalPosition = GetSpawnPosition();
		}
	}


	public void OnArenaDifficultyIncreased(int arena_difficulty)
	{
		double time_off = (0.1 / 12) * arena_difficulty;
		time_off = Math.Min(time_off, 0.7);

		enemy_timer.WaitTime = base_spawn_time - time_off;

		if (arena_difficulty == 6)
			enemy_table.AddItem(ghost_enemy_scene, 15);
		else if (arena_difficulty == 18)
			enemy_table.AddItem(flying_enemy_scene, 7);

		if ((arena_difficulty % 12) == 0)
			number_to_spawn++;
	}
}
