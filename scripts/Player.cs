using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	[Export] public ArenaTimeManager area_time_manager { get; set; }

	public MetaProgression meta_progression = null;
	public HealthComponent health_component = null;
	public VelocityComponent velocity_component = null;
	public Timer damage_interval_timer = null;
	public ProgressBar health_bar = null;
	public Node abilities = null;
	public GameEvents game_events = null;
	public AnimatedSprite2D animated_sprite = null;

	public int bodies_colliding = 0;
	public float base_speed = 0;


    public override void _Ready()
    {
		meta_progression = GetNode<MetaProgression>("/root/MetaProgression");

		health_component = GetNode<HealthComponent>("HealthComponent");
		health_component.HealthChanged += OnHealthChanged;
		health_component.HealthDecreased += OnHealthDecreased;

		velocity_component = GetNode<VelocityComponent>("VelocityComponent");
		base_speed = velocity_component.max_speed;

		health_bar = GetNode<ProgressBar>("HealthBar");
		abilities = GetNode<Node>("Abilities");
		UpdateHealthDisplay();

		damage_interval_timer = GetNode<Timer>("DamageIntervalTimer");
		damage_interval_timer.Timeout += OnDamageIntervalTimerTimeout;

		game_events = GetNode<GameEvents>("/root/GameEvents");
		game_events.AbilityUpgradeAdded += OnAbilityUpgradeAdded;

		area_time_manager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;

		GetNode<Area2D>("CollisionArea2D").BodyEntered += OnBodyEntered;
		GetNode<Area2D>("CollisionArea2D").BodyExited += OnBodyExited;

		animated_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }


    public override void _Process(double delta)
	{
		Vector2 movement_vector = GetMovementVector();
		Vector2 direction = movement_vector.Normalized();
		velocity_component.AccelerateInDirection(direction);
		velocity_component.Move(this);

		UpdateAnimation(movement_vector);
	}


	public void UpdateAnimation(Vector2 movement_vector)
    {
        if (movement_vector == Vector2.Zero)
        {
            animated_sprite.Play("idle");
        }
        else if (Mathf.Abs(movement_vector.X) > Mathf.Abs(movement_vector.Y))
        {
            if (movement_vector.X > 0)
                animated_sprite.Play("walk_right");
            else
                animated_sprite.Play("walk_left");
        }
        else
        {
            if (movement_vector.Y > 0)
                animated_sprite.Play("walk_down");
            else
                animated_sprite.Play("walk_up");
        }
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
		GetNode<RandomAudioComponent2D>("RandomHitAudioComponent").PlayRandom();
	}


	public void OnBodyExited(Node2D other_body)
	{
		bodies_colliding--;
	}
	

	public void OnHealthDecreased()
	{
		game_events.EmitPlayerDamaged();
	}


	public void OnHealthChanged()
	{
		UpdateHealthDisplay();
	}


	public void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades)
	{
		if (upgrade is Ability ability)
		{
			var ability_instance = ability.ability_controller_scene.Instantiate();
			abilities.AddChild(ability_instance);
		}
		else if (upgrade.id == "player_speed")
		{
			Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
			int current_quantity = (int) upgrade_data["quantity"];
			velocity_component.max_speed = base_speed + (current_quantity * 0.1f);
		}
	}


	public void OnArenaDifficultyIncreased(int arena_difficulty)
	{
		int health_regeneration_quantity = meta_progression.GetUpgradeCount("health_regeneration");
		
		if (health_regeneration_quantity > 0)
		{
			bool is_thirty_second_interval = (arena_difficulty % 6) == 0;
			if (is_thirty_second_interval)
			{
				health_component.Heal(health_regeneration_quantity);
			}
		}
	}
}
