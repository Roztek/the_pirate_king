using Godot;

public partial class ExperienceBar : CanvasLayer
{
	[Export] public ExperienceManager experience_manager { get; set; }

	public ProgressBar progress_bar = null;


    public override void _Ready()
    {
		progress_bar = GetNode<ProgressBar>("MarginContainer/ProgressBar");
		progress_bar.Value = 0;

		experience_manager.ExperienceUpdated += OnExperienceUpdated;
    }


	public void OnExperienceUpdated(float current_experience, float target_experience)
	{
		float percent = current_experience / target_experience;
		progress_bar.Value = percent;
	}
}
