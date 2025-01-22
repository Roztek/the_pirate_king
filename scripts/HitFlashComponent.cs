using Godot;

public partial class HitFlashComponent : Node
{
    [Export] public HealthComponent health_component { get; set; }
    [Export] public Sprite2D sprite { get; set; }
    [Export] public ShaderMaterial hit_flash_material { get; set; }

    public Tween hit_flash_tween = null;


    public override void _Ready()
    {
        health_component.HealthDecreased += OnHealthDecreased;
    }


    public void OnHealthDecreased()
    {
        if (hit_flash_tween != null && hit_flash_tween.IsValid())
            hit_flash_tween.Kill();

        (sprite.Material as ShaderMaterial).SetShaderParameter("lerp_percent", 1.0);

        hit_flash_tween = CreateTween();
        hit_flash_tween.TweenProperty(sprite.Material, "shader_parameter/lerp_percent", 0.0f, 0.25f)
            .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
    }
}
