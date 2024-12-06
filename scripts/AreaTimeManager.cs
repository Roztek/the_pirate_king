using Godot;
using System;

public partial class AreaTimeManager : Node
{
    [Export] PackedScene end_screen_scene;

    private Timer _timer = null;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }


    public double GetTimeElapsed()
    {
        return _timer.WaitTime - _timer.TimeLeft;
    }


    public void OnTimerTimeout()
    {
        if(end_screen_scene.Instantiate() is not EndScreen end_screen_instance)
            return;

        AddChild(end_screen_instance);
    }
}
