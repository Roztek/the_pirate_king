using Godot;
using System;

public partial class GameCamera : Camera2D
{
	Vector2 target_position = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MakeCurrent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AcquireTarget();
		GlobalPosition = GlobalPosition.Lerp(target_position, 1.0f - Mathf.Exp(-(float)delta * 10));
	}

	public void AcquireTarget() 
	{
		var player_nodes = GetTree().GetNodesInGroup("player");

		if (player_nodes.Count > 0) {
			Node2D player = player_nodes[0] as Node2D;    // Cast to Node2D

			if (player != null) {						  // Check if the cast was successful 
				target_position = player.GlobalPosition;  // Use GlobalPosition property
			}
		}
	}
}
