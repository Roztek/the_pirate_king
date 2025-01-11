using Godot;
using System;

public partial class ScreenTransition : CanvasLayer
{
    [Signal] public delegate void TransitionedHalfwayEventHandler();

    public AnimationPlayer animation_player = null;

    bool skip_emit = false;


    public override void _Ready()
    {
        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
    }


    public async void Transition()
    {
        animation_player.Play("default");
        await ToSignal(animation_player, "animation_finished");
        skip_emit = true;
        animation_player.PlayBackwards("default");
    }


    public void EmitTransitionedHalfway()
    {
        if (skip_emit)
        {
            skip_emit = false;
            return;
        }

        EmitSignal(SignalName.TransitionedHalfway);
    }
}
