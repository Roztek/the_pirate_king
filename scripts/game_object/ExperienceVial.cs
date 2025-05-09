using Godot;
using System;

public partial class ExperienceVial : Node2D
{
	public CollisionShape2D collision_shape_2d = null;
	public Sprite2D sprite = null;

	private Vector2 _startPosition;


	public override void _Ready()
	{
		collision_shape_2d = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");

		sprite = GetNode<Sprite2D>("Sprite2D");

		GetNode<Area2D>("Area2D").AreaEntered += OnAreaEntered;
	}


	public void TweenCollect(float percent)
    {
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;
		
		GlobalPosition = _startPosition.Lerp(player.GlobalPosition, percent);
		var start_direction = player.GlobalPosition - _startPosition;

		var target_rotation = start_direction.Angle() + Mathf.DegToRad(90);
		Rotation = (float) Mathf.LerpAngle(Rotation, target_rotation, 1 - Math.Exp(-2 *GetProcessDeltaTime()));
	}


	public void Collect()
	{
		GetNode<GameEvents>("/root/GameEvents").EmitExperienceVialCollected(1);
		QueueFree();
	}


	public void DisableCollision()
	{
		collision_shape_2d.Disabled = true;
	}


	public void OnAreaEntered(Area2D area)
	{
		new Callable(this, nameof(DisableCollision)).CallDeferred();

		_startPosition = GlobalPosition;

        Tween tween = CreateTween();
		tween.SetParallel();
        tween.TweenMethod(new Callable(this, nameof(TweenCollect)), 0.0, 1.0, 0.5)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);
        tween.TweenProperty(sprite, "scale", Vector2.Zero, 0.05).SetDelay(0.45);
		tween.Chain();
		tween.TweenCallback(new Callable(this, nameof(Collect)));

		GetNode<RandomAudioComponent2D>("RandomPickupAudioComponent").PlayRandom();
	}
}
