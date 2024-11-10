using Godot;
using System;

public partial class VialDropComponent : Node
{
    [Export] PackedScene vial_scene = null;

    [Export] HealthComponent _health_component = null;

    public override void _Ready()
	{
		vial_scene = GetNode<PackedScene>("ExperienceVial");

        _health_component = GetNode<HealthComponent>("HealthComponent");
        _health_component.Died += OnDied;
	}


    public void OnDied()
    {
        if (vial_scene == null)
            return;

        if (!(Owner is Node2D))
            return;

        var spawn_position = (Owner as Node2D)?.GlobalPosition ?? Vector2.Zero;
        var vial_instance = vial_scene.Instantiate() as Node2D;

        GetParent().AddChild(vial_instance);
		vial_instance.GlobalPosition = spawn_position;
    }
}
