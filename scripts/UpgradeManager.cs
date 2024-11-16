using Godot;
using System;
using Godot.Collections;

public partial class UpgradeManager : Node
{
	[Export] public Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }
	[Export] public PackedScene upgrade_screen_scene { get; set; }

	public Dictionary current_upgrades = new Dictionary();


    public override void _Ready()
    {
		experience_manager.LevelUp += OnLevelUp;
    }


	public void OnLevelUp(int new_level)
	{
		AbilityUpgrade chosen_upgrade = upgrade_pool.PickRandom();
		if (chosen_upgrade == null)
			return;

		var upgrade_screen_instance = upgrade_screen_scene.Instantiate() as UpgradeScreen;
		AddChild(upgrade_screen_instance);
		upgrade_screen_instance.SetAbilityUpgrades(new Array<AbilityUpgrade> { chosen_upgrade });

		ApplyUpgrade(chosen_upgrade);
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

		GD.Print(current_upgrades);
	}
}
