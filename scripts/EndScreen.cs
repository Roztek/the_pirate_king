using Godot;

public partial class EndScreen : CanvasLayer
{
    public ScreenTransition screen_transition = null;
    public Label title_label = null;
    public Label description_label = null;
    public PanelContainer panel_container = null;


    public override void _Ready()
    {
        GetTree().Paused = true;

        screen_transition = GetNode<ScreenTransition>("/root/ScreenTransition");

        title_label = GetNode<Label>("%TitleLabel");
        description_label = GetNode<Label>("%DescriptionLabel");

        panel_container = GetNode<PanelContainer>("%PanelContainer");
        panel_container.PivotOffset = panel_container.Size / 2;

        Tween tween = CreateTween();
        tween.TweenProperty(panel_container, "scale", Vector2.Zero, 0);
        tween.TweenProperty(panel_container, "scale", Vector2.One, 0.3f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
        
        GetNode<Button>("%ContinueButton").Pressed += OnContinueButtonPressed;
        GetNode<Button>("%MenuButton").Pressed += OnMenuButtonPressed;
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
            GetNode<AudioStreamPlayer>("DefeatAudio").Play();
        else
            GetNode<AudioStreamPlayer>("VictoryAudio").Play();
    }


    public void OnContinueButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/meta_menu.tscn", true);
    }


    public void OnMenuButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/main_menu.tscn", true);
    }
}
