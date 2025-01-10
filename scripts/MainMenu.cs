using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    public Button play_button = null;
    public Button options_button = null;
    public Button quit_button = null;


    public override void _Ready()
    {
        play_button = GetNode<Button>("%PlayButton");
        play_button.Pressed += OnPlayButtonPressed;

        options_button = GetNode<Button>("%OptionsButton");
        options_button.Pressed += OnOptionsButtonPressed;

        quit_button = GetNode<Button>("%QuitButton");
        quit_button.Pressed += OnQuitButtonPressed;
    }


    public void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
    }


    public void OnOptionsButtonPressed()
    {
        
    }


    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
