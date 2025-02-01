using Godot;
using System;

public partial class ArenaTimeUi : CanvasLayer
{
	[Export] public ArenaTimeManager ArenaTimeManager { get; set; } 

	private Label _time_label = null;


    public override void _Ready()
    {
		_time_label = GetNode<Label>("%TimeLabel");
	}


	public override void _Process(double delta)
	{
		if (ArenaTimeManager == null)
			return;

		double time_elapsed = ArenaTimeManager.GetTimeElapsed();
		_time_label.Text = FormatTimeToString(time_elapsed);
	}


	public string FormatTimeToString(double seconds)
	{
		int minutes = (int)(seconds / 60);
    	int seconds_remaining = (int)(seconds % 60); 
 
		return $"{minutes:D2}:{seconds_remaining:D2}";
	}
}
