using Godot;

public partial class ScreenTransition : CanvasLayer
{
    [Signal] public delegate void TransitionedHalfwayEventHandler();

    public AnimationPlayer animation_player = null;

    private bool _skip_emit = false;


    public override void _Ready()
    {
        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
    }


    public async void Transition()
    {
        animation_player.Play("default");
        await ToSignal(animation_player, "animation_finished");
        _skip_emit = true;
        animation_player.PlayBackwards("default");
    }


    public void EmitTransitionedHalfway()
    {
        if (_skip_emit)
        {
            _skip_emit = false;
            return;
        }

        EmitSignal(SignalName.TransitionedHalfway);
    }


    public async void TransitionToScene(string scene_path, bool unpause_game = false)
    {
        Transition();
        await ToSignal(this, "TransitionedHalfway");

        if (unpause_game)
            GetTree().Paused = false;

        GetTree().ChangeSceneToFile(scene_path);
    }
}
