using Godot;
using System;
using Godot.Collections;

public partial class RandomHitAudioComponent : AudioStreamPlayer2D
{
    [Export] public Array<AudioStream> streams { get; set; } 


    public void PlayRandom()
    {
        if (streams == null || streams.Count == 0)
            return;

        Stream = streams.PickRandom();
        Play();
    }
}
