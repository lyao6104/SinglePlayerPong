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
        var direction = Vector2.Zero;

        if (Input.IsActionPressed("Paddle Up"))
        {
            direction.Y -= 1;
        }

        if (Input.IsActionPressed(("Paddle Down")))
        {
            direction.Y += 1;
        }

        if (direction.Length() > 0)
        {
            direction = direction.Normalized() * Speed;
        }

        MoveAndCollide(direction * Speed);
        Position = new Vector2(_xPos, Position.Y);
    }
}
