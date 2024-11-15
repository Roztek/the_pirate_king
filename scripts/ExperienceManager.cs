using Godot;
using System;

public partial class ExperienceManager : Node
{
    [Signal] public delegate void ExperienceUpdatedEventHandler(float current_experience, float target_experience);
    [Signal] public delegate void LevelUpEventHandler(int new_level);

    // scales target experience on level up (more exp to lvl on lvl up)
    const int TARGET_EXPERIENCE_GROWTH = 5;
   
    public float current_experience = 0;
    public float target_experience = 5;
    public int current_level = 0;


    public override void _Ready()
    {
        GameEvents game_events = (GameEvents) GetNode("/root/GameEvents");
        game_events.ExperienceVialCollected += OnExperienceVialCollected;
    }


    public void IncrementExperience(float number)
    {
        current_experience = Math.Min(current_experience + number, target_experience);
        EmitSignal(SignalName.ExperienceUpdated, current_experience, target_experience);
        if (current_experience >= target_experience) 
        {
            current_level += 1;
            target_experience += TARGET_EXPERIENCE_GROWTH;
            current_experience = 0;
            EmitSignal(SignalName.ExperienceUpdated, current_experience, target_experience);
            EmitSignal(SignalName.LevelUp, current_level);
        }
    }


    public void OnExperienceVialCollected(float number)
    {
        IncrementExperience(number);
    }
}
