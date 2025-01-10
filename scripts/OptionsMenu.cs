using Godot;
using System;

public partial class OptionsMenu : CanvasLayer
{
    [Signal] public delegate void BackPressedEventHandler();

    public Button window_button = null;
    public Button back_button = null;
    public HSlider sfx_slider = null;
    public HSlider music_slider = null;


    public override void _Ready()
    {
        window_button = GetNode<Button>("%WindowButton");
        window_button.Pressed += OnWindowButtonPressed;

        back_button = GetNode<Button>("%BackButton");
        back_button.Pressed += OnBackButtonPressed;

        sfx_slider = GetNode<HSlider>("%SFXSlider");
        sfx_slider.ValueChanged += (value) => OnAudioSliderChanged("sfx", (float) value);

        music_slider = GetNode<HSlider>("%MusicSlider");
        music_slider.ValueChanged += (value) => OnAudioSliderChanged("music", (float) value);

        UpdateDisplay();
    }


    public void UpdateDisplay()
    {
        window_button.Text = "Windowed";
        var mode = DisplayServer.WindowGetMode();
        if (mode != DisplayServer.WindowMode.Fullscreen)
            window_button.Text = "Fullscreen";
        
        sfx_slider.Value = GetBusVolumePrecent("sfx");
        music_slider.Value = GetBusVolumePrecent("music");
    }


    public float GetBusVolumePrecent(string bus_name)
    {
        int bus_index = AudioServer.GetBusIndex(bus_name);
        float volume_db = AudioServer.GetBusVolumeDb(bus_index);

        return Mathf.DbToLinear(volume_db);
    }


    public void SetBusVolumePrecent(string bus_name, float percent)
    {
        int bus_index = AudioServer.GetBusIndex(bus_name);
        float volume_db = Mathf.LinearToDb(percent);
        AudioServer.SetBusVolumeDb(bus_index, volume_db);
    }


    public void OnWindowButtonPressed()
    {
        var mode = DisplayServer.WindowGetMode();
        if (mode != DisplayServer.WindowMode.Fullscreen)
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        
        UpdateDisplay();
    }


    public void OnBackButtonPressed()
    {
        EmitSignal(SignalName.BackPressed);
    }


    public void OnAudioSliderChanged(string bus_name, float value)
    {
        SetBusVolumePrecent(bus_name, value);
    }
}
