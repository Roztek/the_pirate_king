using Godot;

public partial class MainMenu : CanvasLayer
{
    public PackedScene options_menu_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/options_menu.tscn");

    public ScreenTransition screen_transition = null;

    
    public override void _Ready()
    {
        screen_transition = GetNode<ScreenTransition>("/root/ScreenTransition");

        GetNode<Button>("%PlayButton").Pressed += OnPlayButtonPressed;

        GetNode<Button>("%UpgradesButton").Pressed += OnUpgradesButtonPressed;

        GetNode<Button>("%OptionsButton").Pressed += OnOptionsButtonPressed;

        GetNode<Button>("%QuitButton").Pressed += OnQuitButtonPressed;
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


    public async void OnQuitButtonPressed()
    {
        screen_transition.Transition();
        await ToSignal(screen_transition, "TransitionedHalfway");
        GetTree().Quit();
    }


    public void OnBackPressed(OptionsMenu options_menu_instance)
    {
        options_menu_instance.QueueFree();
    }
}
