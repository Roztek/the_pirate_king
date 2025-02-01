using Godot;
using Godot.Collections;

public partial class MetaMenu : CanvasLayer
{
    [Export] public Array<MetaUpgrade> upgrades { get; set; } 

    public PackedScene meta_upgrade_card_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/meta_upgrade_card.tscn");

    public ScreenTransition screen_transition = null;
    public GridContainer grind_container = null;


    public override void _Ready()
    {
        screen_transition = GetNode<ScreenTransition>("/root/ScreenTransition");

        grind_container = GetNode<GridContainer>("%GridContainer");

        GetNode<Button>("%BackButton").Pressed += OnBackButtonPressed;

        foreach(MetaUpgrade upgrade in upgrades)
        {
            if (meta_upgrade_card_scene.Instantiate() is not MetaUpgradeCard meta_upgrade_card_instance)
				return;

            grind_container.AddChild(meta_upgrade_card_instance);
            meta_upgrade_card_instance.SetMetaUpgrade(upgrade);
        }
    }


    public void OnBackButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/main_menu.tscn");
    }
}
