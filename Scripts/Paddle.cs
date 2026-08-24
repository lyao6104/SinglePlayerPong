using Godot;

public partial class Paddle : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 2;

    private Vector2 _screenSize;
    private float _xPos;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _xPos = Position.X;
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("Paddle Up"))
        {
            velocity.Y -= 1;
        }

        if (Input.IsActionPressed(("Paddle Down")))
        {
            velocity.Y += 1;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
        }

        MoveAndCollide(velocity * Speed);
        Position = new Vector2(_xPos, Position.Y);
    }
}
