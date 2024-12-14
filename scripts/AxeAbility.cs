using Godot;
using System;

public partial class AxeAbility : Node2D
{
    const int MAX_RADIUS = 100;

    public Vector2 base_rotation = Vector2.Right;

    public HitboxComponent hitbox_component = null;


    public override void _Ready()
    {
        hitbox_component = GetNode<HitboxComponent>("HitboxComponent");
        
        base_rotation = Vector2.Right.Rotated((float) (new Random().NextDouble() * Math.PI * 2));

        var tween = CreateTween();
        tween.TweenMethod(new Callable(this, nameof(MyTweenMethod)), 0.0f, 2.0f, 3.0f);

        tween.Finished += OnTweenFinished;
    }


    public void MyTweenMethod(float rotations)
    {
        float percent = rotations / 2;
        float current_radius = percent * MAX_RADIUS;
        Vector2 current_direction = base_rotation.Rotated(rotations * (float) Math.Tau);

        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        GlobalPosition = player.GlobalPosition + (current_direction * current_radius);
    }


    public void OnTweenFinished()
    {
        QueueFree();
    }
}
