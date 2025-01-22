using Godot;

public partial class FlyingEnemy : CharacterBody2D
{
    public VelocityComponent velocity_component = null;
    public Node2D visuals = null;


    public override void _Ready()
    {
        velocity_component = GetNode<VelocityComponent>("VelocityComponent");
        
        visuals = GetNode<Node2D>("Visuals");

        GetNode<HurtboxComponent>("HurtboxComponent").Hit += OnHit;
    }


    public override void _Process(double delta)
    {
        velocity_component.accelerate_to_player();
		velocity_component.Move(this);

        var move_sign = Mathf.Sign(Velocity.X);
		if (move_sign != 0)
			visuals.Scale = new Vector2(move_sign, 1);  
    }


    public void OnHit()
	{
		GetNode<RandomAudioComponent2D>("RandomHitAudioComponent").PlayRandom();
	}
}
