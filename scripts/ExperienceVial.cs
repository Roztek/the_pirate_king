using Godot;
using System;

public partial class ExperienceVial : Node2D
{
	public override void _Ready()
	{
		Area2D experience_vial_area = GetNode<Area2D>("Area2D");
        experience_vial_area.AreaEntered += OnAreaEntered;
	}


	public void OnAreaEntered(Area2D area)
	{
		GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.EmitExperienceVialCollected(1);
		QueueFree();
	}
}
