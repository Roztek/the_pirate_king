using Godot;
using System;

public partial class Vignette : CanvasLayer
{
    public AnimationPlayer animation_player = null;

    
    public override void _Ready()
    {
        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");

        GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.PlayerDamaged += OnPlayerDamaged;
    }


    public void OnPlayerDamaged()
    {
        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
        animation_player.Play("hit");
    }
}
