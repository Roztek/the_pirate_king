using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class UpgradeManager : Node
{
	[Export] public Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }
	[Export] public PackedScene upgrade_screen_scene { get; set; }

	public const int TOTAL_UPGRADES = 2;

	public Dictionary current_upgrades = new Dictionary();


    public override void _Ready()
    {
		experience_manager.LevelUp += OnLevelUp;
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

		if (upgrade.max_quantity > 0)
		{
			Dictionary upgrade_data = (Dictionary) current_upgrades[upgrade.id];
			int current_quantity = (int) upgrade_data["quantity"];
			if (current_quantity == upgrade.max_quantity)
			{
				upgrade_pool = new Array<AbilityUpgrade>(upgrade_pool.Where(u => Filter(u, upgrade)));
			}

		}

		GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.EmitAbilityUpgradeAdded(upgrade, current_upgrades);
	}


	public Array<AbilityUpgrade> PickUpgrades()
	{
		Array<AbilityUpgrade> chosen_upgrades = new Array<AbilityUpgrade>();
		Array<AbilityUpgrade> filtered_upgrades = upgrade_pool.Duplicate();

		for (int i = 0; i < TOTAL_UPGRADES; i++)
		{
			if (filtered_upgrades.Count() == 0)
				break;
			AbilityUpgrade chosen_upgrade = filtered_upgrades[(int)(GD.Randi() % filtered_upgrades.Count)];
			chosen_upgrades.Add(chosen_upgrade);
			filtered_upgrades = new Array<AbilityUpgrade>(filtered_upgrades.Where(upgrade => Filter(upgrade, chosen_upgrade)));
		}

		return chosen_upgrades;
	}


	public bool Filter(AbilityUpgrade upgrade, AbilityUpgrade chosen_upgrades)
	{
		return upgrade.id != chosen_upgrades.id;
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
