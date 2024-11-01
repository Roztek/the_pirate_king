using Godot;
using System;

public partial class EnemyManager : Node
{
	const int SPAWN_RADIUS = 376;

	[Export]
    public PackedScene BasicEnemyScene { get; set; }


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

		var spawn_position = player.GlobalPosition + offset;

		var enemy = BasicEnemyScene.Instantiate() as Node2D;
		GetParent().AddChild(enemy);
		enemy.GlobalPosition = spawn_position;
	}
}
