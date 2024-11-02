using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	const int MAX_SPEED = 40;


	public override void _Ready()
	{
		Area2D hitbox = GetNode<Area2D>("Area2D");
		hitbox.AreaEntered += OnAreaEntered;
	}


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


	public void OnAreaEntered(Area2D area)
	{
			QueueFree();
	}
}
