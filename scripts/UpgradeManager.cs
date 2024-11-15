using Godot;
using System;

public partial class UpgradeManager : Node
{
	[Export] public Godot.Collections.Array<AbilityUpgrade> upgrade_pool { get; set; } 
	[Export] public ExperienceManager experience_manager { get; set; }

    public override void _Ready()
    {
		experience_manager.LevelUp += OnLevelUp;
    }


	public void OnLevelUp()
	{

	}
}
