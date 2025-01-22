using Godot;

public partial class VelocityComponent : Node
{
    [Export] public float max_speed { get; set; }
    [Export] public float acceleration { get; set; }

    private Vector2 _velocity = Vector2.Zero;


    public void Move(CharacterBody2D character_body)
    {
        character_body.Velocity = _velocity;
        character_body.MoveAndSlide();
        _velocity = character_body.Velocity;
    }


    public void AccelerateInDirection(Vector2 direction)
    {
        var desired_velocity = direction * max_speed;
        float weight_lerp = 1.0f - Mathf.Exp(-acceleration * (float) GetPhysicsProcessDeltaTime());
        _velocity = _velocity.Lerp(desired_velocity, weight_lerp);
    }


    public void Decelerate()
    {
        AccelerateInDirection(Vector2.Zero);
    }


    public void AccelerateToPlayer()
    {
        var owner_node2d = Owner as Node2D;
        if (owner_node2d == null)
            return;
        
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        var direction = (player.GlobalPosition - owner_node2d.GlobalPosition).Normalized();
        AccelerateInDirection(direction);
    }
}
