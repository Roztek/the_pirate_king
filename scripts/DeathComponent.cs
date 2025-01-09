using Godot;
using System;

public partial class DeathComponent : Node2D
{
    [Export] public HealthComponent health_component { get; set; }
    [Export] public Sprite2D sprite { get; set; }

    public AnimationPlayer animation_player = null;
    public GpuParticles2D gpu_particles_2d = null;
    public RandomAudioComponent random_audio_component = null;


    public override void _Ready()
    {
        health_component = GetParent().GetNode<HealthComponent>("HealthComponent");
        health_component.Died += OnDied;

        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");

        gpu_particles_2d = GetNode<GpuParticles2D>("GPUParticles2D");
        gpu_particles_2d.Texture = sprite.Texture;

        random_audio_component = GetNode<RandomAudioComponent>("RandomHitAudioComponent");
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
        animation_player.Play("default");

        random_audio_component.PlayRandom();
    }
}
