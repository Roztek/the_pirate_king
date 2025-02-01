using Godot;

public partial class GameCamera : Camera2D
{
	public Vector2 target_position = Vector2.Zero;


	public override void _Ready()
	{
		MakeCurrent();
	}


	public override void _Process(double delta)
	{
		AcquireTarget();
		float weight_lerp = 1.0f - Mathf.Exp(-(float) delta * 20);
		GlobalPosition = GlobalPosition.Lerp(target_position, weight_lerp);
	}


	public void AcquireTarget() 
	{
		var player_nodes = GetTree().GetNodesInGroup("player");

		if (player_nodes.Count > 0) {
			Node2D player = player_nodes[0] as Node2D;

			if (player != null)
				target_position = player.GlobalPosition;
		}
	}
}
