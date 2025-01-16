using Godot;
using System;

public partial class VialDropComponent : Node
{
    [Export] private PackedScene _vial_scene;
    [Export] public HealthComponent health_component { get; set; }
    [Export(PropertyHint.Range, "0, 1")] private float _spawn_rate;

    public MetaProgression meta_progress = null;


    public override void _Ready()
	{
        meta_progress = (MetaProgression) GetNode("/root/MetaProgression");

        health_component = GetParent().GetNode<HealthComponent>("HealthComponent");
        health_component.Died += OnDied;
	}


    public void OnDied()
    {
        CallDeferred(nameof(SpawnVial));
    }


    private void SpawnVial()
    {
        float adjusted_drop_percent = _spawn_rate;
        var experience_gain_upgrade_count = meta_progress.GetUpgradeCount("experience_gain");
        if (experience_gain_upgrade_count > 0)
            adjusted_drop_percent += 0.1f;

        float random_number = (float) new Random().NextDouble();
        if (random_number > adjusted_drop_percent)
            return;

        if (_vial_scene == null || Owner is not Node2D owner_node)
            return;

        var spawn_position = owner_node.GlobalPosition;

        // Instantiate the vial and check if it was successful
        if (_vial_scene.Instantiate() is not Node2D vial_instance)
            return;

        Node2D entities_layer = GetTree().GetFirstNodeInGroup("entities_layer") as Node2D;
        entities_layer.AddChild(vial_instance);
        vial_instance.GlobalPosition = spawn_position;      
    }
}
