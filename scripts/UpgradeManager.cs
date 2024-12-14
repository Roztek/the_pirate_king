using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class UpgradeManager : Node
{
	[Export] public Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }
	[Export] public PackedScene upgrade_screen_scene { get; set; }

	public Dictionary current_upgrades = new Dictionary();


    public override void _Ready()
    {
		experience_manager.LevelUp += OnLevelUp;

		foreach (var upgrade in upgrade_pool)
		{
			GD.Print(upgrade);
		}
    }


	public void ApplyUpgrade(AbilityUpgrade upgrade)
	{
		var has_upgrade = current_upgrades.ContainsKey(upgrade.id);
		if (!has_upgrade)
		{
			current_upgrades[upgrade.id] = new Dictionary()
			{
				{ "resource", upgrade },
				{ "quantity", 1 },
			};
		}
		else
		{
			Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
			int current_quantity = (int) upgrade_data["quantity"];
			upgrade_data["quantity"] = current_quantity + 1;
		}

		GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.EmitAbilityUpgradeAdded(upgrade, current_upgrades);

		//GD.Print(current_upgrades);
	}


	public Array<AbilityUpgrade> PickUpgrades()
	{
		Array<AbilityUpgrade> chosen_upgrades = new Array<AbilityUpgrade>();
		Array<AbilityUpgrade> filteredUpgrades = upgrade_pool.Duplicate();

		for (int i = 0; i < 2; i++)
		{
			AbilityUpgrade chosen_upgrade = filteredUpgrades.PickRandom();
			chosen_upgrades.Append(chosen_upgrade);
			filteredUpgrades.Remove(chosen_upgrade);
		}

		return chosen_upgrades;
	}


	public void OnLevelUp(int new_level)
	{
		var upgrade_screen_instance = upgrade_screen_scene.Instantiate() as UpgradeScreen;
		AddChild(upgrade_screen_instance);
		Array<AbilityUpgrade> chosen_upgrades = PickUpgrades();
		upgrade_screen_instance.SetAbilityUpgrades(chosen_upgrades);
		upgrade_screen_instance.UpgradeSelected += OnUpgradeSelected;
	}


	public void OnUpgradeSelected(AbilityUpgrade upgrade)
	{
		ApplyUpgrade(upgrade);
	}
}
