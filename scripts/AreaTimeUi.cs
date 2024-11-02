using Godot;
using System;

public partial class AreaTimeUi : CanvasLayer
{
	[Export]
    public AreaTimeManager AreaTimeManager { get; set; } 


	private Label _label;


    public override void _Ready()
    {
		_label = GetNode<Label>("%Label");
	}


	public override void _Process(double delta)
	{
		if (AreaTimeManager == null)
			return;

		double time_elapsed = AreaTimeManager.GetTimeElapsed();
		_label.Text = FormatTimeToString(time_elapsed);
	}

	public string FormatTimeToString(double seconds)
	{
		int minutes = (int)Math.Floor(seconds / 60);
		int seconds_left = (int)Math.Floor(seconds % 60); 
 
		//return string.Format("{0:D2}:{1:D2}", minutes, seconds_left);
		return $"{minutes:D2}:{seconds_left:D2}";
	}
}
