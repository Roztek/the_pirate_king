using Godot;
using System;

public partial class UpgradeManager : Node
{
	[Export] public Godot.Collections.Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }

	public Godot.Collections.Dictionary current_upgrades = new Godot.Collections.Dictionary();


    public override void _Ready()
    {
		experience_manager.LevelUp += OnLevelUp;
    }


	public void OnLevelUp(int new_level)
	{
		AbilityUpgrade chosen_upgrade = upgrade_pool.PickRandom();
		if (chosen_upgrade == null)
			return;
		
		var has_upgrade = current_upgrades.ContainsKey(chosen_upgrade.id);
		if (!has_upgrade)
		{
			current_upgrades[chosen_upgrade.id] = new Godot.Collections.Dictionary()
			{
				{ "resource", chosen_upgrade },
				{ "quantity", 1 },
			};
		}
		else
		{
			Godot.Collections.Dictionary upgrade_data = (Godot.Collections.Dictionary) current_upgrades[chosen_upgrade.id];
			int current_quantity = (int) upgrade_data["quantity"];
			upgrade_data["quantity"] = current_quantity + 1;
		}

		GD.Print(current_upgrades);
	}
}
