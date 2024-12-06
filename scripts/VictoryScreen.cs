using Godot;
using System;

public partial class VictoryScreen : CanvasLayer
{
    public Button restart_button = null;
    public Button quit_button = null;


    public override void _Ready()
    {
        GetTree().Paused = true;

        restart_button = GetNode<Button>("%RestartButton");
        restart_button.Pressed += OnRestartButtonPressed;

        quit_button = GetNode<Button>("%QuitButton");
        quit_button.Pressed += OnQuitButtonPressed;
    }


    public void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
    }


    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
