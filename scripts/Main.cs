using Godot;
using System;

public partial class Main : Node
{
    [Export] public PackedScene end_screen_scene { get; set; }

    public Player player = null;

    public override void _Ready()
    {
        player = GetNode<Player>("%Player");
        player.health_component.Died += OnPlayerDied;
    }


    public void OnPlayerDied()
    {
        if(end_screen_scene.Instantiate() is not EndScreen end_screen_instance)
            return;

        AddChild(end_screen_instance);
        end_screen_instance.SetDefeat();
    }
}
