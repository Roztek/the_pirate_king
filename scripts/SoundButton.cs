using Godot;
using System;

public partial class SoundButton : Button
{
    public RandomAudioComponent random_audio_component = null;

    public override void _Ready()
    {
        this.Pressed += OnPressed;

        random_audio_component = GetNode<RandomAudioComponent>("RandomClickAudioComponent");
    }


    public void OnPressed()
    {
        random_audio_component.PlayRandom();
    }
}
