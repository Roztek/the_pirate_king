using Godot;
using Godot.Collections;

public partial class UpgradeManager : Node
{
	[Export] public ExperienceManager experience_manager { get; set; }
	[Export] public PackedScene upgrade_screen_scene { get; set; }

	public Resource upgrade_axe = ResourceLoader.Load("res://resources/upgrades/axe.tres");
	public Resource upgrade_anvil = ResourceLoader.Load("res://resources/upgrades/anvil.tres");
	public Resource upgrade_axe_damage = ResourceLoader.Load("res://resources/upgrades/axe_damage.tres");
	public Resource upgrade_sword_speed = ResourceLoader.Load("res://resources/upgrades/sword_speed.tres");
	public Resource upgrade_sword_damage = ResourceLoader.Load("res://resources/upgrades/sword_damage.tres");
	public Resource upgrade_player_speed = ResourceLoader.Load("res://resources/upgrades/player_speed.tres");
	public Resource upgrade_anvil_amount = ResourceLoader.Load("res://resources/upgrades/anvil_amount.tres");

	private Dictionary<string, Dictionary> current_upgrades = new Dictionary<string, Dictionary>();

	public WeightedTable upgrade_pool = new WeightedTable();
	

    public override void _Ready()
    {
		upgrade_pool.AddItem(upgrade_axe, 10);
		upgrade_pool.AddItem(upgrade_anvil, 10);
		upgrade_pool.AddItem(upgrade_sword_speed, 10);
		upgrade_pool.AddItem(upgrade_sword_damage, 10);
		upgrade_pool.AddItem(upgrade_player_speed, 5);

		experience_manager.LevelUp += OnLevelUp;
    }


	public void ApplyUpgrade(AbilityUpgrade upgrade)
	{
		bool has_upgrade = current_upgrades.ContainsKey(upgrade.id);
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
		GetNode<GameEvents>("/root/GameEvents").EmitAbilityUpgradeAdded(upgrade, current_upgrades);
	}


	public void UpdateUpgradePool(AbilityUpgrade chosen_upgrade)
	{
		if (chosen_upgrade == upgrade_axe)
		{
			upgrade_pool.AddItem(upgrade_axe_damage, 10);
		}
		else if (chosen_upgrade == upgrade_anvil)
		{
			upgrade_pool.AddItem(upgrade_anvil_amount, 5);
		}
	}


	public Array<AbilityUpgrade> PickUpgrades()
	{
		Array<AbilityUpgrade> chosen_upgrades = new Array<AbilityUpgrade>();

		for (int i = 0; i < 2; i++)
		{
			AbilityUpgrade chosen_upgrade = null;

			while (chosen_upgrade == null || chosen_upgrades.Contains(chosen_upgrade))
			{
				chosen_upgrade = upgrade_pool.PickRandom() as AbilityUpgrade;

				if (chosen_upgrade == null)
					break;
			}

			if (chosen_upgrade != null)
			{
				chosen_upgrades.Add(chosen_upgrade);
			}
		}

		return chosen_upgrades;
	}


	public void OnLevelUp(int new_level)
	{
		if (upgrade_screen_scene.Instantiate() is not UpgradeScreen upgrade_screen_instance)
			return;

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
