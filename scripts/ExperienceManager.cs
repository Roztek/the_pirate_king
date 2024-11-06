using Godot;
using System;

public partial class ExperienceManager : Node
{
    private float current_experience = 0;


    public override void _Ready()
    {
        GameEvents game_events = GetNode<GameEvents>("GameEvents");
        game_events.ExperienceVialCollected += OnExperienceVialCollected;
    }


    public void IncrementExperience(float number)
    {
        current_experience += number;
        Console.WriteLine(current_experience);
    }


    public void OnExperienceVialCollected(float number)
    {
        IncrementExperience(number);
    }
}
