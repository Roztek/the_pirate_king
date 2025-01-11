using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    public ScreenTransition screen_transition = null;
    public Button resume_button = null;
    public Button options_button = null;
    public Button menu_button = null;
    public AnimationPlayer animation_player = null;
    public PanelContainer panel_container = null;

    public bool is_closing = false;

    public PackedScene options_menu_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/options_menu.tscn");


    public override void _Ready()
    {
        GetTree().Paused = true;

        screen_transition = (ScreenTransition) GetNode("/root/ScreenTransition");

        resume_button = GetNode<Button>("%ResumeButton");
        resume_button.Pressed += OnResumeButtonPressed;

        options_button = GetNode<Button>("%OptionsButton");
        options_button.Pressed += OnOptionsButtonPressed;

        menu_button = GetNode<Button>("%MenuButton");
        menu_button.Pressed += OnMenuButtonPressed;

        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
        animation_player.Play("default");

        panel_container = GetNode<PanelContainer>("%PanelContainer");

        panel_container.PivotOffset = panel_container.Size / 2;
        Tween tween = CreateTween();
        tween.TweenProperty(panel_container, "scale", Vector2.Zero, 0);
        tween.TweenProperty(panel_container, "scale", Vector2.One, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            close();
            GetTree().Root.SetInputAsHandled();
        }
    }


    public async void close()
    {
        if (is_closing)
            return;
        is_closing = true;

        animation_player.PlayBackwards("default");

        Tween tween = CreateTween();
        tween.TweenProperty(panel_container, "scale", Vector2.One, 0);
        tween.TweenProperty(panel_container, "scale", Vector2.Zero, 0.3f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);

        await ToSignal(animation_player, "animation_finished");

        GetTree().Paused = false;
        QueueFree();
    }


    public void OnResumeButtonPressed()
    {
        close();
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


    public async void OnMenuButtonPressed()
    {
        screen_transition.Transition();
        await ToSignal(screen_transition, "TransitionedHalfway");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/ui/main_menu.tscn");
    }


    public void OnBackPressed(OptionsMenu options_menu_instance)
    {
        options_menu_instance.QueueFree();
    }
}
