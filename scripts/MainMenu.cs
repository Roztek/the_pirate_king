using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    public ScreenTransition screen_transition = null;
    public Button play_button = null;
    public Button upgrades_button = null;
    public Button options_button = null;
    public Button quit_button = null;

    public PackedScene options_menu_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/options_menu.tscn");


    public override void _Ready()
    {
        screen_transition = (ScreenTransition) GetNode("/root/ScreenTransition");

        play_button = GetNode<Button>("%PlayButton");
        play_button.Pressed += OnPlayButtonPressed;

        upgrades_button = GetNode<Button>("%UpgradesButton");
        upgrades_button.Pressed += OnUpgradesButtonPressed;

        options_button = GetNode<Button>("%OptionsButton");
        options_button.Pressed += OnOptionsButtonPressed;

        quit_button = GetNode<Button>("%QuitButton");
        quit_button.Pressed += OnQuitButtonPressed;
    }


    public void OnPlayButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/main/main.tscn");
    }


    public void OnUpgradesButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/meta_menu.tscn");
    }


    public async void OnOptionsButtonPressed()
    {
        screen_transition.Transition();
        await ToSignal(screen_transition, "TransitionedHalfway");

        if (options_menu_scene.Instantiate() is not OptionsMenu options_menu_instance)
            return;
        AddChild(options_menu_instance);
        options_menu_instance.BackPressed += () => OnBackPressed(options_menu_instance);
    }


    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }


    public void OnBackPressed(OptionsMenu options_menu_instance)
    {
        options_menu_instance.QueueFree();
    }
}
