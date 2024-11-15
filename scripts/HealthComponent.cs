using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HealthComponent : Node
{
    [Signal] public delegate void DiedEventHandler();

    [Export] public float max_health { get; set; }

    private float _current_health;


    public override void _Ready()
    {
        _current_health = max_health;
    }


    public void Damage(float damage_amount)
    {
        // this allows _current_health to never drop below zero
        _current_health = Math.Max(_current_health - damage_amount, 0);
        CallDeferred(nameof(CheckDeath));
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
