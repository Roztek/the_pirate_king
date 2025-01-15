using Godot;
using System;

public partial class ArenaTimeManager : Node
{
    [Signal] public delegate void ArenaDifficultyIncreasedEventHandler(int arena_difficulty);

    [Export] public PackedScene end_screen_scene { get; set; }

    const int DIFFICULTY_INTERVAL = 5;

    int arena_difficulty = 0;

    private Timer _timer = null;


    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }


    public override void _Process(double delta)
    {
        double next_time_target = _timer.WaitTime - ((arena_difficulty + 1) * DIFFICULTY_INTERVAL);

        if (_timer.TimeLeft <= next_time_target)
        {
            arena_difficulty++;
            EmitSignal(SignalName.ArenaDifficultyIncreased, arena_difficulty);
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
        
        MetaProgression meta_progression = (MetaProgression) GetNode("/root/MetaProgression");
        meta_progression.Save();
    }
}
