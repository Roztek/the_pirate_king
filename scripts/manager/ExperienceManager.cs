using Godot;
using System;

public partial class ExperienceManager : Node
{
    [Signal] public delegate void ExperienceUpdatedEventHandler(float current_experience, float target_experience);
    [Signal] public delegate void LevelUpEventHandler(int new_level);

    // scales target experience on level up (more exp to lvl on lvl up)
    private const int TARGET_EXPERIENCE_GROWTH = 5;
   
    public float current_experience = 0;
    public float target_experience = 5;
    public int current_level = 0;


    public override void _Ready()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnExperienceVialCollected;
    }
    
    
    public override void _ExitTree()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected -= OnExperienceVialCollected;
    }


    public void IncrementExperience(float number)
    {
        current_experience = Math.Min(current_experience + number, target_experience);
        
        if (current_experience >= target_experience) 
        {
            current_level += 1;
            target_experience += TARGET_EXPERIENCE_GROWTH;
            current_experience = 0;
            EmitSignal(SignalName.LevelUp, current_level);
        }

        EmitSignal(SignalName.ExperienceUpdated, current_experience, target_experience);
    }


    public void OnExperienceVialCollected(float number)
    {
        IncrementExperience(number);
    }
}
