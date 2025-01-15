using Godot;
using System;
using Godot.Collections;

public partial class MetaMenu : CanvasLayer
{
    [Export] public Array<MetaUpgrade> upgrades { get; set; } 

    public GridContainer grind_container = null;

    public PackedScene meta_upgrade_card_scene = (PackedScene) ResourceLoader.Load("res://scenes/ui/meta_upgrade_card.tscn");


    public override void _Ready()
    {
        grind_container = GetNode<GridContainer>("%GridContainer");

        foreach(MetaUpgrade upgrade in upgrades)
        {
            var meta_upgrade_card_instance = meta_upgrade_card_scene.Instantiate() as MetaUpgradeCard;
            grind_container.AddChild(meta_upgrade_card_instance);
            meta_upgrade_card_instance.SetMetaUpgrade(upgrade);
        }
    }
}
