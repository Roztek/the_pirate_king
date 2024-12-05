using Godot;
using System;

public partial class Player : CharacterBody2D
{
	const int MAX_SPEED = 125;
	const int ACCELERATION_SMOOTHING = 25;

	public int bodies_colliding = 0;

	public HealthComponent health_component = null;
	public Timer damage_interval_timer = null;


    public override void _Ready()
    {
		health_component = GetNode<HealthComponent>("HealthComponent");

		damage_interval_timer = GetNode<Timer>("DamageIntervalTimer");
		damage_interval_timer.Timeout += OnDamageIntervalTimerTimeout;

        Area2D body_entered = GetNode<Area2D>("CollisionArea2D");
		body_entered.BodyEntered += OnBodyEntered;

		Area2D body_exited = GetNode<Area2D>("CollisionArea2D");
		body_exited.BodyExited += OnBodyExited;
    }


    public override void _Process(double delta)
	{
		Vector2 movement_vector = GetMovementVector();
		Vector2 direction = movement_vector.Normalized();
		var target_velocity = direction * MAX_SPEED;

		Velocity = Velocity.Lerp(target_velocity, 1.0f - Mathf.Exp(-(float)delta * ACCELERATION_SMOOTHING));

		MoveAndSlide();
	}


	static private Vector2 GetMovementVector()
	{
		float x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		float y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		return new Vector2(x, y);
	}


	public void DealDamage()
	{
		// don't deal damage when player is not colliding or timer is running
		if (bodies_colliding == 0 || !damage_interval_timer.IsStopped())
			return;

		health_component.Damage(1);
		damage_interval_timer.Start();
		GD.Print(health_component.current_health);
	}


	public void OnDamageIntervalTimerTimeout()
	{
		DealDamage();
	}


	public void OnBodyEntered(Node2D other_body)
	{
		bodies_colliding++;
		DealDamage();
	}


	public void OnBodyExited(Node2D other_body)
	{
		bodies_colliding--;
	}
}
