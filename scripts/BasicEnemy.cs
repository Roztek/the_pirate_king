using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	const int MAX_SPEED = 40;

	public HealthComponent health_component = null;
	public Node2D visuals = null;


	public override void _Ready()
	{
		health_component = GetNode<HealthComponent>("HealthComponent");

		visuals = GetNode<Node2D>("Visuals");
	}


	public override void _Process(double delta)
	{
		Vector2 direction = GetDirectionToPlayer();
		Velocity = direction * MAX_SPEED;
		MoveAndSlide();

		var move_sign = Mathf.Sign(Velocity.X);
		if (move_sign != 0)
			visuals.Scale = new Vector2(move_sign, 1);  
	}


	public Vector2 GetDirectionToPlayer()
	{
		Node2D player_node = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (player_node != null) 
			return (player_node.GlobalPosition - GlobalPosition).Normalized();

		return Vector2.Zero;
	}
}
