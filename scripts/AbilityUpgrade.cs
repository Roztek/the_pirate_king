using Godot;
using System;
using System.ComponentModel;

public partial class AbilityUpgrade : Resource
{
	[Export] public string id { get; set; }
	[Export] public string name { get; set; }
	[Export] public string description { get; set; }
}
