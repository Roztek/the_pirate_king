using Godot;
using System;

public partial class HurtboxComponent : Area2D
{
    [Export] public HealthComponent health_component { get; set; }

    public override void _Ready()
    {
		this.AreaEntered += OnAreaEntered;
    }


    public void OnAreaEntered(Area2D area)
	{
		if (!(area is HitboxComponent))
            return;

        if (health_component == null)
            return;

        HitboxComponent hitbox_component = area as HitboxComponent;
        health_component.Damage(hitbox_component.damage);
	}
}
