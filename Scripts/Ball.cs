using Godot;

public partial class Ball : RigidBody2D
{
    [Export] public float StartingForce = 10;

    private bool _initialized;
    private RandomNumberGenerator _random;

    public override void _Ready()
    {
        _random = new RandomNumberGenerator();
    }

    public override void _Process(double delta)
    {
        if (!_initialized && Input.IsActionJustPressed("Start"))
        {
            var direction = Vector2.Right.Rotated(_random.Randf() * Mathf.Pi * 2);
            ApplyImpulse(direction * StartingForce);
            _initialized = true;
        }
    }
}
