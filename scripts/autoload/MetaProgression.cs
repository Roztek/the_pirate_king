using Godot;
using Godot.Collections;

public partial class MetaProgression : Node
{
    private const string SAVE_FILE_PATH = "user://game.save";

    public Dictionary save_data = new Dictionary
    {
        { "meta_upgrade_currency", 0 },
        { "meta_upgrades", new Dictionary() }
    };


    public override void _Ready()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnExperienceVialCollected;
        LoadSaveFile();
    }


    public override void _ExitTree()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected -= OnExperienceVialCollected;
    }


    public void LoadSaveFile()
    {
        if (!FileAccess.FileExists(SAVE_FILE_PATH))
            return;
        
        var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Read);
        save_data = (Dictionary) file.GetVar(); 
    }


    public void Save()
    {
        var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Write);
        file.StoreVar(save_data);
    }


    public void AddMetaUpgrade(MetaUpgrade upgrade)
    {
        var meta_upgrades = (Dictionary) save_data["meta_upgrades"];
        
        bool has_upgrade = meta_upgrades.ContainsKey(upgrade.id);
        if (!has_upgrade)
        {
            meta_upgrades[upgrade.id] = new Dictionary
            {
                { "quantity", 0 }
            };
        }

        var upgrade_data = (Dictionary) meta_upgrades[upgrade.id];
        int current_quantity = (int) upgrade_data["quantity"];
        
        if (current_quantity < upgrade.max_quantity)
            upgrade_data["quantity"] = current_quantity + 1;

        Save();
    }


    public int GetUpgradeCount(string upgrade_id)
    {
        Dictionary meta_upgrades = (Dictionary) save_data["meta_upgrades"];
        
        if (meta_upgrades != null && meta_upgrades.ContainsKey(upgrade_id))
        {
            Dictionary upgrade_data = (Dictionary) meta_upgrades[upgrade_id];
            
            if (upgrade_data != null && upgrade_data.ContainsKey("quantity"))
            {
                return (int) upgrade_data["quantity"];
            }
        }
        
        return 0;
    }


    public void OnExperienceVialCollected(float number)
    {
        int current_currency = (int) save_data["meta_upgrade_currency"];
		save_data["meta_upgrade_currency"] = current_currency + 1;
    }
}
