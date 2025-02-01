using Godot;

public partial class MetaUpgrade : Resource
{
    [Export] public string id { get; set; }
	[Export] public string name { get; set; }
	[Export(PropertyHint.MultilineText)] public string description { get; set; }
	[Export] public int max_quantity { get; set; }
	[Export] public int experience_cost { get; set; }
}
