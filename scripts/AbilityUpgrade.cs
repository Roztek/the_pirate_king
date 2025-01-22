using Godot;

public partial class AbilityUpgrade : Resource
{
	[Export] public string id { get; set; }
	[Export] public string name { get; set; }
	[Export(PropertyHint.MultilineText)] public string description { get; set; }
	[Export] public int max_quantity { get; set; }
}
