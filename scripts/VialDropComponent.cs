using Godot;
using System;

public partial class VialDropComponent : Node
{
    [Export] PackedScene vial_scene = null;

    [Export] HealthComponent _health_component;

    public override void _Ready()
	{
        _health_component = GetParent().GetNode<HealthComponent>("HealthComponent");
        _health_component.Died += OnDied;
	}


    public void OnDied()
    {
        CallDeferred(nameof(SpawnVial));
    }


    private void SpawnVial()
    {
        if (vial_scene == null || Owner is not Node2D owner_node)
            return;

        var spawn_position = owner_node.GlobalPosition;

        // Instantiate the vial and check if it was successful.
        if (vial_scene.Instantiate() is not Node2D vial_instance)
            return;

        GetParent().AddChild(vial_instance);
        vial_instance.GlobalPosition = spawn_position;
    }
}
