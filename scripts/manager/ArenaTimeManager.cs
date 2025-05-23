using Godot;
using System;

public partial class ArenaTimeManager : Node
{
    [Signal] public delegate void ArenaDifficultyIncreasedEventHandler(int arena_difficulty);

    [Export] public PackedScene end_screen_scene { get; set; }

    private const int DIFFICULTY_INTERVAL = 5;

    private Timer _timer = null;
    private int _arena_difficulty = 0;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }


    public override void _Process(double delta)
    {
        double next_time_target = _timer.WaitTime - ((_arena_difficulty + 1) * DIFFICULTY_INTERVAL);

        if (_timer.TimeLeft <= next_time_target)
        {
            _arena_difficulty++;
            EmitSignal(SignalName.ArenaDifficultyIncreased, _arena_difficulty);
        }
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
        end_screen_instance.SetVictory();
        GetNode<MetaProgression>("/root/MetaProgression").Save();
    }
}
