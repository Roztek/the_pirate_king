using Godot;

public partial class DeathComponent : Node2D
{
    [Export] public HealthComponent health_component { get; set; }
    [Export] public Sprite2D sprite { get; set; }


    public override void _Ready()
    {
        GetParent().GetNode<HealthComponent>("HealthComponent").Died += OnDied;

        GetNode<GpuParticles2D>("GPUParticles2D").Texture = sprite.Texture;
    }


    public void OnDied()
    {
        var owner_node = Owner as Node2D;
        if (owner_node == null)
            return;
            
        Vector2 spawn_position = owner_node.GlobalPosition;

        var entities = GetTree().GetFirstNodeInGroup("entities_layer");
        GetParent().RemoveChild(this);
        entities.AddChild(this);

        GlobalPosition = spawn_position;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("default");
        GetNode<RandomAudioComponent2D>("RandomHitAudioComponent").PlayRandom();
    }
}
