using Godot;
using System;

public partial class Vignette : CanvasLayer
{
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
        GetNode<GameEvents>("/root/GameEvents").PlayerDamaged += OnPlayerDamaged;
    }


    public override void _ExitTree()
    {
        GetNode<GameEvents>("/root/GameEvents").PlayerDamaged -= OnPlayerDamaged;
    }


    public void OnPlayerDamaged()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("hit");
    }
}
