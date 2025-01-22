using Godot;

public partial class Music : AudioStreamPlayer
{
    public Timer timer = null;

    public override void _Ready()
    {
        this.Finished += OnFinished;

        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
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
