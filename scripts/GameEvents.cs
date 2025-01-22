using Godot;
using Godot.Collections;
using System;

public partial class GameEvents : Node
{
    [Signal] public delegate void ExperienceVialCollectedEventHandler(float number);
    [Signal] public delegate void AbilityUpgradeAddedEventHandler(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades);
    [Signal] public delegate void PlayerDamagedEventHandler();


    public void EmitExperienceVialCollected(float number)
    {
        EmitSignal(SignalName.ExperienceVialCollected, number);
    }


    public void EmitAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> current_upgrades)
    {
        EmitSignal(SignalName.AbilityUpgradeAdded, upgrade, current_upgrades);
    }


    public void EmitPlayerDamaged()
    {
        EmitSignal(SignalName.PlayerDamaged);
    }
}
