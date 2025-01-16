using Godot;
using System;
using Godot.Collections;

public partial class MetaMenu : CanvasLayer
{
    [Export] public Array<MetaUpgrade> upgrades { get; set; } 

    public ScreenTransition screen_transition = null;
    public GridContainer grind_container = null;
    public Button back_button = null;

    public PackedScene meta_upgrade_card_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/meta_upgrade_card.tscn");


    public override void _Ready()
    {
        screen_transition = (ScreenTransition) GetNode("/root/ScreenTransition");

        grind_container = GetNode<GridContainer>("%GridContainer");

        back_button = GetNode<Button>("%BackButton");
        back_button.Pressed += OnBackButtonPressed;

        foreach(MetaUpgrade upgrade in upgrades)
        {
            var meta_upgrade_card_instance = meta_upgrade_card_scene.Instantiate() as MetaUpgradeCard;
            grind_container.AddChild(meta_upgrade_card_instance);
            meta_upgrade_card_instance.SetMetaUpgrade(upgrade);
        }
    }


    public void OnBackButtonPressed()
    {
        screen_transition.TransitionToScene("res://scenes/ui/main_menu.tscn");
    }
}
