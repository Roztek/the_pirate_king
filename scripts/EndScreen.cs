using Godot;
using System;

public partial class EndScreen : CanvasLayer
{
    public Button restart_button = null;
    public Button quit_button = null;
    public Label title_label = null;
    public Label description_label = null;


    public override void _Ready()
    {
        GetTree().Paused = true;

        restart_button = GetNode<Button>("%RestartButton");
        restart_button.Pressed += OnRestartButtonPressed;

        quit_button = GetNode<Button>("%QuitButton");
        quit_button.Pressed += OnQuitButtonPressed;

        title_label = GetNode<Label>("%TitleLabel");

        description_label = GetNode<Label>("%DescriptionLabel");
    }

    
    public void SetDefeat()
    {
        title_label.Text = "Defeat";
        description_label.Text = "You lost!";
    }


    // public void SetVictory()
    // {
    //     title_label.Text = "Victory";
    //     description_label.Text = "You won!";
    // }


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
