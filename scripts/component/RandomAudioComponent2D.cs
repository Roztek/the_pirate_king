using Godot;
using Godot.Collections;

public partial class RandomAudioComponent2D : AudioStreamPlayer2D
{
    [Export] public Array<AudioStream> streams { get; set; } 
    [Export] public bool randomize_pitch = true;
    [Export] public float min_pitch = 0.9f;
    [Export] public float max_pitch = 1.1f;


    public void PlayRandom()
    {
        if (streams == null || streams.Count == 0)
            return;

        if (randomize_pitch)
            PitchScale = (float) GD.RandRange(min_pitch, max_pitch);
        else
            PitchScale = 1;

        Stream = streams.PickRandom();
        Play();
    }
}
