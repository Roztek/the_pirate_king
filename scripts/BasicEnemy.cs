using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	[Export] public VelocityComponent velocity_component;


	public Node2D visuals = null;


	public override void _Ready()
	{
		visuals = GetNode<Node2D>("Visuals");
	}


	public override void _Process(double delta)
	{
		velocity_component.accelerate_to_player();
		velocity_component.Move(this);

		var move_sign = Mathf.Sign(Velocity.X);
		if (move_sign != 0)
			visuals.Scale = new Vector2(move_sign, 1);  
	}
}
