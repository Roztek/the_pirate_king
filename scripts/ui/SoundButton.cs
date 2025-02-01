using Godot;

public partial class SoundButton : Button
{
    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }


    public void OnPressed()
    {
        GetNode<RandomAudioComponent>("RandomClickAudioComponent").PlayRandom();
    }
}
