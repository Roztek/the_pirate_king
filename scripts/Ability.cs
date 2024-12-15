using Godot;
using System;

public partial class Ability : AbilityUpgrade
{
    [Export] public PackedScene ability_controller_scene { get; set; }
}
