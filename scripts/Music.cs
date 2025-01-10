using Godot;
using System;

public partial class Music : AudioStreamPlayer
{
    public Timer timer = null;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;

        this.Finished += OnFinished;
    }


    public void OnFinished()
    {
        timer.Start();
    }


    public void OnTimerTimeout()
    {
        Play();
    }
}
