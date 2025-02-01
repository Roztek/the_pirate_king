using Godot;
using System;
using Godot.Collections;

public partial class MetaUpgradeCard : PanelContainer
{
    public MetaProgression meta_progress = null;
    public Label name_label = null;
    public Label description_label = null;
    public Label progress_label = null;
    public Label count_label = null;
    public AnimationPlayer card_animation_player = null;
    public ProgressBar progress_bar = null;
    public Button purchase_button = null;
    public MetaUpgrade meta_upgrade = null;
    
    
    public override void _Ready()
    {
        meta_progress = GetNode<MetaProgression>("/root/MetaProgression");

        name_label = GetNode<Label>("%NameLabel");
        description_label = GetNode<Label>("%DescriptionLabel");
        progress_label = GetNode<Label>("%ProgressLabel");
        count_label = GetNode<Label>("%CountLabel");

        card_animation_player = GetNode<AnimationPlayer>("%CardAnimationPlayer");
        
        progress_bar = GetNode<ProgressBar>("%ProgressBar");

        purchase_button = GetNode<SoundButton>("%PurchaseButton");
        purchase_button.Pressed += OnPurchaseButtonPressed;
    }


    public void SetMetaUpgrade(MetaUpgrade upgrade)
    {
        this.meta_upgrade = upgrade;
        name_label.Text = upgrade.name;
        description_label.Text = upgrade.description;
        UpdateProgress();
    }


    public void UpdateProgress()
    {
        int quantity = 0;
        Dictionary upgrades = (Dictionary) meta_progress.save_data["meta_upgrades"];
        if (upgrades.ContainsKey(meta_upgrade.id))
        {
            Dictionary upgrade_data = (Dictionary) upgrades[meta_upgrade.id];
            quantity = (int) upgrade_data["quantity"];
        }
        
        int currency = (int) meta_progress.save_data["meta_upgrade_currency"];
        currency = Math.Max(0, currency);
        int experience_cost = meta_upgrade.experience_cost;

        float percent = (float) currency / experience_cost;
        percent = Math.Min(percent, 1);

        bool is_maxed = quantity >= meta_upgrade.max_quantity;
        // Disable the purchase button if player doesn't have enough experience, or maxed
        purchase_button.Disabled = percent < 1 || is_maxed;
        if (is_maxed)
            purchase_button.Text = "Maxed";

        progress_bar.Value = percent; 
        progress_label.Text = currency.ToString() + "/" + experience_cost.ToString();
        count_label.Text = $"X{quantity}";
    }


    public void SelectCard()
    {
        card_animation_player.Play("selected");
    }


    public void OnPurchaseButtonPressed()
    {
        if (meta_upgrade == null)
            return;

        meta_progress.AddMetaUpgrade(meta_upgrade);
        meta_progress.save_data["meta_upgrade_currency"] = (int) meta_progress.save_data["meta_upgrade_currency"] - meta_upgrade.experience_cost;
        meta_progress.Save();
        GetTree().CallGroup("meta_upgrade_card", "UpdateProgress");
        card_animation_player.Play("selected");
    }
}
