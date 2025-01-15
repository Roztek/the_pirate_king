using Godot;
using System;
using Godot.Collections;

public partial class MetaProgression : Node
{
    public Dictionary save_data = new Dictionary
    {
        { "meta_upgrade_currency", 0 },
        { "meta_upgrades", new Dictionary() }
    };


    public override void _Ready()
    {
        GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
        game_events.ExperienceVialCollected += OnExperienceVialCollected;
    }


    public void AddMetaUpgrade(MetaUpgrade upgrade)
    {

        var meta_upgrades = (Dictionary) save_data["meta_upgrades"];
        bool has_upgrade = save_data.ContainsKey(upgrade.id);
        if (!has_upgrade)
        {
            meta_upgrades[upgrade.id] = new Dictionary
            {
                { "quantity", 0 }
            };
        }

        var upgrade_data = (Dictionary) meta_upgrades[upgrade.id];
        upgrade_data["quantity"] = (int) upgrade_data["quantity"] + 1;
    }


    public void OnExperienceVialCollected(float number)
    {
        int current_currency = (int) save_data["meta_upgrade_currency"];
		save_data["meta_upgrade_currency"] = current_currency + 1;
    }
}
