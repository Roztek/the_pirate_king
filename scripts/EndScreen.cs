using Godot;
using System;
using System.Threading.Tasks;

public partial class EndScreen : CanvasLayer
{
    public ScreenTransition screen_transition = null;
    public Button continue_button = null;
    public Button menu_button = null;
    public Label title_label = null;
    public Label description_label = null;
    public PanelContainer panel_container = null;


    public override void _Ready()
    {
        GetTree().Paused = true;

        screen_transition = (ScreenTransition) GetNode("/root/ScreenTransition");

        continue_button = GetNode<Button>("%ContinueButton");
        continue_button.Pressed += OnContinueButtonPressed;

        menu_button = GetNode<Button>("%MenuButton");
        menu_button.Pressed += OnMenuButtonPressed;

        title_label = GetNode<Label>("%TitleLabel");

        description_label = GetNode<Label>("%DescriptionLabel");

        panel_container = GetNode<PanelContainer>("%PanelContainer");

        panel_container.PivotOffset = panel_container.Size / 2;
        Tween tween = CreateTween();
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


    public void OnContinueButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/meta_menu.tscn");
        GetTree().Paused = false;
    }


    public void OnMenuButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/main_menu.tscn");
        GetTree().Paused = false;
    }
}
