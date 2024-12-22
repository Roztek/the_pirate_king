using Godot;
using System;

public partial class HurtboxComponent : Area2D
{
    [Export] public HealthComponent health_component { get; set; }
    [Export] public PackedScene floating_text_scene { get; set; }


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

        var floating_text_instance = floating_text_scene.Instantiate() as FloatingDamage;
        if (floating_text_instance != null)
        {
            GetTree().GetFirstNodeInGroup("foreground_layer")?.AddChild(floating_text_instance);

            floating_text_instance.GlobalPosition = GlobalPosition + (Vector2.Up * 16);
            floating_text_instance.Start(hitbox_component.damage.ToString());
        }
	}
}
