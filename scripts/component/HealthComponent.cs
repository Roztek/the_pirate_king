using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void HealthChangedEventHandler();
    [Signal] public delegate void HealthDecreasedEventHandler();

    [Export] public float max_health { get; set; }

    private float _current_health;


    public override void _Ready()
    {
        _current_health = max_health;
    }


    public void Damage(float damage_amount)
    {
        // this allows _current_health to never drop below zero
        _current_health = Math.Clamp(_current_health - damage_amount, 0, max_health);

        EmitSignal(SignalName.HealthChanged);
        if (damage_amount > 0)
            EmitSignal(SignalName.HealthDecreased);

        CallDeferred(nameof(CheckDeath));
    }


    public void Heal(int heal_amount)
    {
        // this looks strange, but it's just healing the given health points
        Damage(-heal_amount);
    }


    public float GetHealthPercent()
    {
        if (_current_health <= 0)
            return 0;
        
        return Math.Min(_current_health / max_health, 1);
    }


    public void CheckDeath()
    {
        if (_current_health == 0)
        {
            EmitSignal(SignalName.Died);
            Owner.QueueFree();
        }
    }
}
