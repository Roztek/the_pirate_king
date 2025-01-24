using Godot;

public partial class AnchorAbility : Node2D
{
    public HitboxComponent hitbox_component = null;


    public override void _Ready()
    {
        hitbox_component = GetNode<HitboxComponent>("HitboxComponent");
    }
}
