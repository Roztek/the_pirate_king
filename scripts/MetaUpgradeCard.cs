using Godot;
using System;
using System.Threading.Tasks;

public partial class MetaUpgradeCard : PanelContainer
{
    private Label _name_label = null;
    private Label _description_label = null;
    public AnimationPlayer card_animation_player = null;
    public AnimationPlayer hover_animation_player = null;
    
    
    public override void _Ready()
    {
        _name_label = GetNode<Label>("%NameLabel");
        _description_label = GetNode<Label>("%DescriptionLabel");

        card_animation_player = GetNode<AnimationPlayer>("%CardAnimationPlayer");
        hover_animation_player = GetNode<AnimationPlayer>("%HoverAnimationPlayer");

        GuiInput += OnGuiInput;
    }


    public void SetMetaUpgrade(MetaUpgrade upgrade)
    {
        _name_label.Text = upgrade.name;
        _description_label.Text = upgrade.description;
    }


    public void SelectCard()
    {
        card_animation_player.Play("selected");
    }


    public void OnGuiInput(InputEvent input)
    {
        if (input.IsActionPressed("left_click"))
            SelectCard();
    }
}
