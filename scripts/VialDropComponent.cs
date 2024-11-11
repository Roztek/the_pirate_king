using Godot;
using System;

public partial class VialDropComponent : Node
{
    [Export] private PackedScene vial_scene;

    [Export] private HealthComponent _health_component;

    [Export(PropertyHint.Range, "0, 1")] private float _spawn_rate;

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
        float random_number = (float) new Random().NextDouble();
        if (random_number > _spawn_rate)
            return;

        if (vial_scene == null || Owner is not Node2D owner_node)
            return;

        var spawn_position = owner_node.GlobalPosition;

        // Instantiate the vial and check if it was successful
        if (vial_scene.Instantiate() is not Node2D vial_instance)
            return;

        GetParent().GetParent().AddChild(vial_instance);
        vial_instance.GlobalPosition = spawn_position;      
    }
}
