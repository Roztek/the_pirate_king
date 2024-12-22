using Godot;
using System;

public partial class FloatingText : Node2D
{
    public Label damage_label = null;


    public override void _Ready()
    {
        damage_label = GetNode<Label>("Label");
    }


    public void Start(string text)
    {
        damage_label.Text = text;

        var tween = CreateTween();
        tween.SetParallel();
        
        tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 16), 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.Chain();
        
        tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 48), 0.5f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(this, "scale", Vector2.Zero, 0.5f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
        tween.Chain();

        tween.TweenCallback(new Callable(this, nameof(OnTweenCompleted)));

        var scale_tween = CreateTween();
        scale_tween.TweenProperty(this, "scale", Vector2.One * 1.5f, 0.15f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        scale_tween.TweenProperty(this, "scale", Vector2.One, 0.15f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
    }


    private void OnTweenCompleted()
    {
        QueueFree();
    }
}
