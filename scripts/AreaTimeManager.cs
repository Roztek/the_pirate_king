using Godot;
using System;

public partial class AreaTimeManager : Node
{
    private Timer _timer = null;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
    }


    public double GetTimeElapsed()
    {
        return _timer.WaitTime - _timer.TimeLeft;
    }
}
