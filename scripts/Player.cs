using Godot;
using Godot.Collections;
using System;

public partial class Player : CharacterBody2D
{
	public int bodies_colliding = 0;
	public float base_speed = 0;

	public HealthComponent health_component = null;
	public Timer damage_interval_timer = null;
	public ProgressBar health_bar = null;
	public Node abilities = null;
	public VelocityComponent velocity_component = null;
	public GameEvents game_events = null;
	public RandomAudioComponent2D random_audio_component = null;


    public override void _Ready()
    {
		velocity_component = GetNode<VelocityComponent>("VelocityComponent");
		base_speed = velocity_component.max_speed;

		health_component = GetNode<HealthComponent>("HealthComponent");
		health_component.HealthChanged += OnHealthChanged;
		
		health_bar = GetNode<ProgressBar>("HealthBar");
		UpdateHealthDisplay();

		damage_interval_timer = GetNode<Timer>("DamageIntervalTimer");
		damage_interval_timer.Timeout += OnDamageIntervalTimerTimeout;

        Area2D body_entered = GetNode<Area2D>("CollisionArea2D");
		body_entered.BodyEntered += OnBodyEntered;

		Area2D body_exited = GetNode<Area2D>("CollisionArea2D");
		body_exited.BodyExited += OnBodyExited;

		game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.AbilityUpgradeAdded += OnAbilityUpgradeAdded;

		abilities = GetNode<Node>("Abilities");

		random_audio_component = GetNode<RandomAudioComponent2D>("RandomHitAudioComponent");
    }


    public override void _Process(double delta)
	{
		Vector2 movement_vector = GetMovementVector();
		Vector2 direction = movement_vector.Normalized();
		velocity_component.accelerate_in_direction(direction);
		velocity_component.Move(this);
	}


	static private Vector2 GetMovementVector()
	{
		float x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		float y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		return new Vector2(x, y);
	}


	public void UpdateHealthDisplay()
	{
		health_bar.Value = health_component.GetHealthPercent();
	}


	public void DealDamage()
	{
		// don't deal damage when player is not colliding or timer is running
		if (bodies_colliding == 0 || !damage_interval_timer.IsStopped())
			return;

		health_component.Damage(1);
		damage_interval_timer.Start();
	}


	public void OnDamageIntervalTimerTimeout()
	{
		DealDamage();
	}


	public void OnBodyEntered(Node2D other_body)
	{
		bodies_colliding++;
		DealDamage();
		random_audio_component.PlayRandom();
	}


	public void OnBodyExited(Node2D other_body)
	{
		bodies_colliding--;
	}
	

	public void OnHealthChanged()
	{
		game_events.EmitPlayerDamaged();
		UpdateHealthDisplay();
	}


	public void OnAbilityUpgradeAdded(AbilityUpgrade ability_upgrade, Dictionary current_upgrades)
	{
		if (ability_upgrade is Ability ability)
		{
			var ability_instance = ability.ability_controller_scene.Instantiate();
			abilities.AddChild(ability_instance);
		}
		else if (ability_upgrade.id == "player_speed")
		{
			Dictionary upgrade_data = (Dictionary) current_upgrades[ability_upgrade.id];
			int current_quantity = (int) upgrade_data["quantity"];
			velocity_component.max_speed = base_speed + (current_quantity * 0.1f);
		}
	}
}
