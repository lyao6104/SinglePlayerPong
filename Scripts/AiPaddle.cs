using Godot;
using System;

public partial class AiPaddle : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 1.5f;

    private Vector2 _screenSize;
    private float _xPos;
    private Ball _ball;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _xPos = Position.X;

        _ball = GetNode<Ball>("../Ball");
    }

    public override void _Process(double delta)
    {
        var direction = Vector2.Zero;

        if (Position.Y > _ball.Position.Y)
        {
            direction.Y = -1;
        }

        if (Position.Y < _ball.Position.Y)
        {
            direction.Y = 1;
        }

        if (direction.Length() > 0)
        {
            direction = direction.Normalized() * Speed;
        }

        MoveAndCollide(direction * Speed);
        Position = new Vector2(_xPos, Position.Y);
    }
}
