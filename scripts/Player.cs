using Godot;
using System;

public partial class Player : CharacterBody2D
{
	const int MAX_SPEED = 200;

	public override void _Ready()
	{
	}


	public override void _Process(double delta)
	{
		Vector2 movement_vector = GetMovementVector();
		Vector2 direction = movement_vector.Normalized();
		Velocity = direction * MAX_SPEED;
		MoveAndSlide();
	}

	static private Vector2 GetMovementVector()
	{
		float x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		float y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		return new Vector2(x, y);
	}
}
