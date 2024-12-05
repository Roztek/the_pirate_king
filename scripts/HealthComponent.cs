using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal] public delegate void DiedEventHandler();

    [Export] public float max_health { get; set; }

    public float current_health;


    public override void _Ready()
    {
        current_health = max_health;
    }


    public void Damage(float damage_amount)
    {
        // this allows _current_health to never drop below zero
        current_health = Math.Max(current_health - damage_amount, 0);
        CallDeferred(nameof(CheckDeath));
    }


    public void CheckDeath()
    {
        if (current_health == 0)
        {
            EmitSignal(SignalName.Died);
            Owner.QueueFree();
        }
    }
}
