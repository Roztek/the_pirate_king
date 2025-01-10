using Godot;
using System;

public partial class EndScreen : CanvasLayer
{
    public Button restart_button = null;
    public Button quit_button = null;
    public Label title_label = null;
    public Label description_label = null;
    public PanelContainer panel_container = null;


    public override void _Ready()
    {
        GetTree().Paused = true;

        restart_button = GetNode<Button>("%RestartButton");
        restart_button.Pressed += OnRestartButtonPressed;

        quit_button = GetNode<Button>("%QuitButton");
        quit_button.Pressed += OnQuitButtonPressed;

        title_label = GetNode<Label>("%TitleLabel");

        description_label = GetNode<Label>("%DescriptionLabel");

        panel_container = GetNode<PanelContainer>("%PanelContainer");

        panel_container.PivotOffset = panel_container.Size / 2;
        var tween = CreateTween();
        tween.TweenProperty(panel_container, "scale", Vector2.Zero, 0);
        tween.TweenProperty(panel_container, "scale", Vector2.One, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }

    
    public void SetVictory()
    {
        title_label.Text = "Victory";
        description_label.Text = "You won!";
        PlayJingle(false);
    }


    public void SetDefeat()
    {
        title_label.Text = "Defeat";
        description_label.Text = "You lost!";
        PlayJingle(true);
    }
    

    public void PlayJingle(bool defeat = false)
    {
        if (defeat)
        {
            AudioStreamPlayer audio_stream_player = GetNode<AudioStreamPlayer>("DefeatAudio");
            audio_stream_player.Play();
        }
        else
        {
            AudioStreamPlayer audio_stream_player = GetNode<AudioStreamPlayer>("VictoryAudio");
            audio_stream_player.Play();
        }
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
