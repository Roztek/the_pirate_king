using Godot;

public partial class AbilityUpgradeCard : PanelContainer
{
    [Signal] public delegate void SelectedEventHandler();

    public Label name_label = null;
    public Label description_label = null;
    public AnimationPlayer card_animation_player = null;
    public AnimationPlayer hover_animation_player = null;

    private bool _disabled = false;
    
    
    public override void _Ready()
    {
        name_label = GetNode<Label>("%NameLabel");
        description_label = GetNode<Label>("%DescriptionLabel");

        card_animation_player = GetNode<AnimationPlayer>("%CardAnimationPlayer");
        hover_animation_player = GetNode<AnimationPlayer>("%HoverAnimationPlayer");

        GuiInput += OnGuiInput;
        MouseEntered += OnMouseEntered;
    }


    public async void PlayIn(float delay = 0)
    {
        Modulate = Colors.Transparent;
        await ToSignal(GetTree().CreateTimer(delay), "timeout");
        card_animation_player.Play("in");
    }


    public void PlayDiscard()
    {
        card_animation_player.Play("discard");
    }


    public void SetAbilityUpgrade(AbilityUpgrade upgrade)
    {
        name_label.Text = upgrade.name;
        description_label.Text = upgrade.description;
    }


    public async void SelectCard()
    {
        _disabled = true;
        card_animation_player.Play("selected");

        foreach (AbilityUpgradeCard other_card in GetTree().GetNodesInGroup("upgrade_card"))
        {
            if (other_card == this)
                continue;

            other_card.PlayDiscard();
        }

        await ToSignal(card_animation_player, "animation_finished");
        EmitSignal(SignalName.Selected);
    }


    public void OnGuiInput(InputEvent input)
    {
        if (_disabled)
            return;

        if (input.IsActionPressed("left_click"))
            SelectCard();
    }


    public void OnMouseEntered()
    {
        if (_disabled)
            return;

        hover_animation_player.Play("hover");
    }
}
