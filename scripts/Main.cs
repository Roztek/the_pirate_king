using Godot;
using System;

public partial class Main : Node
{
    [Export] public PackedScene end_screen_scene { get; set; }

    public Player player = null;

    public PackedScene pause_menu_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/pause_menu.tscn");


    public override void _Ready()
    {
        player = GetNode<Player>("%Player");
        player.health_component.Died += OnPlayerDied;
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            if (pause_menu_scene.Instantiate() is not PauseMenu pause_menu_instance)
                return;
            AddChild(pause_menu_instance);
            GetTree().Root.SetInputAsHandled();
        }
    }


    public void OnPlayerDied()
    {
        if(end_screen_scene.Instantiate() is not EndScreen end_screen_instance)
            return;

        AddChild(end_screen_instance);
        end_screen_instance.SetDefeat();

        MetaProgression meta_progression = (MetaProgression) GetNode("/root/MetaProgression");
        meta_progression.Save();
    }
}
