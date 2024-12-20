using Godot;
using System;

public partial class VelocityComponent : Node
{
    [Export] public int max_speed { get; set; }
    [Export] public float acceleration { get; set; }

    public Vector2 velocity = Vector2.Zero;


    public void Move(CharacterBody2D character_body)
    {
        character_body.Velocity = velocity;
        character_body.MoveAndSlide();
        velocity = character_body.Velocity;
    }


    public void accelerate_in_direction(Vector2 direction)
    {
        var desired_velocity = direction * max_speed;
        velocity = velocity.Lerp(desired_velocity, 1.0f - Mathf.Exp(-acceleration * (float) GetPhysicsProcessDeltaTime()));
    }


    public void decelerate()
    {
        accelerate_in_direction(Vector2.Zero);
    }


    public void accelerate_to_player()
    {
        var owner_node2d = Owner as Node2D;
        if (owner_node2d == null)
            return;
        
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player == null)
            return;

        var direction = (player.GlobalPosition - owner_node2d.GlobalPosition).Normalized();
        accelerate_in_direction(direction);
    }
}
