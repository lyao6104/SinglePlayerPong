using Godot;

public partial class AiPaddle : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 1.5f;
    [Export] public float MaxActionDelay = 0.5f, MinActionDelay = 0.1f;
    [Export] public float ActionThreshold = 1f;

    private Vector2 _screenSize;
    private float _xPos;
    private float _yTarget;

    private Ball _ball;
    private SceneTreeTimer _actionTimer;
    private RandomNumberGenerator _random;
    private Vector2 _direction;

    private void SetTargetY()
    {
        _yTarget = _ball.Position.Y;
        // Over/undershoot a little, otherwise the AI will always lag behind too much
        var maxOvershoot = Mathf.Min(ActionThreshold * Speed, Mathf.Abs(_yTarget - Position.Y));
        if (_yTarget > Position.Y)
        {
            _yTarget += maxOvershoot;
        }
        else
        {
            _yTarget -= maxOvershoot;
        }

        _yTarget += _random.Randf() * maxOvershoot - maxOvershoot / 2;

        _actionTimer = GetTree().CreateTimer(_random.RandfRange(MinActionDelay, MaxActionDelay));
    }

    public override void _Ready()
    {
        _random = new RandomNumberGenerator();

        _screenSize = GetViewportRect().Size;
        _xPos = Position.X;

        _ball = GetNode<Ball>("../Ball");
        _actionTimer = GetTree().CreateTimer(0);
    }

    public override void _Process(double delta)
    {
        if (Mathf.Abs(Position.Y - _ball.Position.Y) > ActionThreshold && _actionTimer.TimeLeft < 1e-6)
        {
            SetTargetY();
        }

        if (Mathf.Abs(Position.Y - _yTarget) > ActionThreshold)
        {
            if (Position.Y > _yTarget)
            {
                _direction.Y = -1;
            }
            else if (Position.Y < _yTarget)
            {
                _direction.Y = 1;
            }
        }
        else
        {
            _direction.Y = 0;
        }

        var velocity = Vector2.Zero;

        if (_direction.Length() > 0)
        {
            velocity = _direction.Normalized() * Speed;
        }

        MoveAndCollide(velocity);
        Position = new Vector2(_xPos, Position.Y);
    }
}
