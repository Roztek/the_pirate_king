using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class UpgradeManager : Node
{
	// [Export] public Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }
	[Export] public PackedScene upgrade_screen_scene { get; set; }

	public const int TOTAL_UPGRADES = 2;

	public Dictionary current_upgrades = new Dictionary();
	public WeightedTable upgrade_pool = new WeightedTable();

	public Resource upgrade_axe = ResourceLoader.Load("res://resources/upgrade/axe.tres");
	public Resource upgrade_axe_damage = ResourceLoader.Load("res://resources/upgrade/axe_damage.tres");
	public Resource upgrade_sword_speed = ResourceLoader.Load("res://resources/upgrade/sword_speed.tres");
	public Resource upgrade_sword_damage = ResourceLoader.Load("res://resources/upgrade/sword_damage.tres");


    public override void _Ready()
    {
		upgrade_pool.AddItem(upgrade_axe, 10);
		upgrade_pool.AddItem(upgrade_sword_speed, 10);
		upgrade_pool.AddItem(upgrade_sword_damage, 10);

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
				upgrade_pool.RemoveItem(upgrade);
			}

		}

		UpdateUpgradePool(upgrade);
		GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
		game_events.EmitAbilityUpgradeAdded(upgrade, current_upgrades);
	}


	public void UpdateUpgradePool(AbilityUpgrade chosen_upgrade)
	{
		if (chosen_upgrade == upgrade_axe)
		{
			upgrade_pool.AddItem(upgrade_axe_damage, 10);
		}

	}


	public Array<AbilityUpgrade> PickUpgrades()
	{
		Array<AbilityUpgrade> chosen_upgrades = new Array<AbilityUpgrade>();

		for (int i = 0; i < TOTAL_UPGRADES; i++)
		{
			if (upgrade_pool.items.Count == chosen_upgrades.Count)
				break;
			var chosen_upgrade = upgrade_pool.PickRandom() as AbilityUpgrade;
			chosen_upgrades.Add(chosen_upgrade);
		}

		return chosen_upgrades;
	}

	// TODO: Decide if I want this code: this code doesn't allowed for an ability to be offered twice

	// public Array<AbilityUpgrade> PickUpgrades()
	// {
	// 	Array<AbilityUpgrade> chosen_upgrades = new Array<AbilityUpgrade>();
	// 	System.Collections.Generic.HashSet<AbilityUpgrade> seen_in_this_round = new System.Collections.Generic.HashSet<AbilityUpgrade>();

	// 	for (int i = 0; i < TOTAL_UPGRADES; i++)
	// 	{
	// 		AbilityUpgrade chosen_upgrade = null;

	// 		while (chosen_upgrade == null || seen_in_this_round.Contains(chosen_upgrade))
	// 		{
	// 			chosen_upgrade = upgrade_pool.PickRandom() as AbilityUpgrade;

	// 			if (chosen_upgrade == null)
	// 				break;
	// 		}

	// 		if (chosen_upgrade != null)
	// 		{
	// 			chosen_upgrades.Add(chosen_upgrade);
	// 			seen_in_this_round.Add(chosen_upgrade);
	// 		}
	// 	}

	// 	return chosen_upgrades;
	// }


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
