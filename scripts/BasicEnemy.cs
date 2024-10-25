using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	const int MAX_SPEED = 25;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction = GetDirectionToPlayer();
		Velocity = direction * MAX_SPEED;
		MoveAndSlide();
	}


	public Vector2 GetDirectionToPlayer()
	{
		Node2D player_node = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (player_node != null) 
			return (player_node.GlobalPosition - GlobalPosition).Normalized();

		return Vector2.Zero;
	}
}
