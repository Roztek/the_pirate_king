using Godot;
using System;

public partial class GhostEnemy : CharacterBody2D
{
    public VelocityComponent velocity_component = null;
    public Node2D visuals = null;
    public RandomHitAudioComponent random_audio_component = null;
    public HurtboxComponent hurtbox_component = null;

    public bool is_moving = false;


    public override void _Ready()
    {
        velocity_component = GetNode<VelocityComponent>("VelocityComponent");

        visuals = GetNode<Node2D>("Visuals");

        random_audio_component = GetNode<RandomHitAudioComponent>("RandomHitAudioComponent");

        hurtbox_component = GetNode<HurtboxComponent>("HurtboxComponent");
		hurtbox_component.Hit += OnHit;
    }


    public override void _Process(double delta)
    {
        if (is_moving)
            velocity_component.accelerate_to_player();
        else
            velocity_component.decelerate();
            
		velocity_component.Move(this);

        var move_sign = Mathf.Sign(Velocity.X);
		if (move_sign != 0)
			visuals.Scale = new Vector2(move_sign, 1);  
    }


    public void SetIsMoving(bool moving)
    {
        is_moving = moving;
    }


    public void OnHit()
	{
		random_audio_component.PlayRandom();
	}
}
