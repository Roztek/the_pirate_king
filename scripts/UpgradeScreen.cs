using Godot;
using Godot.Collections;

public partial class UpgradeScreen : CanvasLayer
{
    [Signal] public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

    [Export] public PackedScene upgrade_card_scene { get; set; }

    public HBoxContainer card_container = null;
    public AnimationPlayer animation_player = null;


    public override void _Ready()
    {
        card_container = GetNode<HBoxContainer>("%CardContainer");

        animation_player = GetNode<AnimationPlayer>("AnimationPlayer");

        GetTree().Paused = true;
    }


    public void SetAbilityUpgrades(Array<AbilityUpgrade> upgrades)
    {
        float delay = 0;

        foreach (AbilityUpgrade upgrade in upgrades) 
        {
            if (upgrade_card_scene.Instantiate() is not AbilityUpgradeCard card_instance)
			    return;

            card_container.AddChild(card_instance);
            card_instance.SetAbilityUpgrade(upgrade);
            card_instance.PlayIn(delay);
            card_instance.Selected += () => OnUpgradeSelectedUi(upgrade);
            delay += 0.2f;
        }
    }


    public async void OnUpgradeSelectedUi(AbilityUpgrade upgrade)
    {
        EmitSignal(SignalName.UpgradeSelected, upgrade);
        animation_player.Play("out");
        await ToSignal(animation_player, "animation_finished");
        GetTree().Paused = false;
        QueueFree();
    }
}
