using System.Linq;
using Godot;

public partial class Ball : RigidBody2D
{
    [Export] public float StartingForce = 10;
    [Export] public Label StartLabel;
    [Export] public Label ScoreLabel;

    private bool _initialized;
    private RandomNumberGenerator _random;
    private Vector2 _startingPos;
    private int _playerScore;
    private int _aiScore;

    public override void _Ready()
    {
        _random = new RandomNumberGenerator();

        var startEvents =
            string.Join(", ", InputMap.ActionGetEvents("Start").Select(inputEvent => inputEvent.AsText()));
        StartLabel.Text = $"Press [{startEvents}] to Start";
    }

    public override void _Process(double delta)
    {
        if (!_initialized && Input.IsActionJustPressed("Start"))
        {
            _startingPos = GlobalPosition;

            // i.e. moves left or right, with an angle of up to 45 degrees up or down
            var leftRight = _random.RandiRange(0, 1) == 0 ? Vector2.Left : Vector2.Right;
            var direction = leftRight.Rotated(_random.Randf() * Mathf.Pi / 2 - Mathf.Pi / 4);
            ApplyImpulse(direction * StartingForce);

            StartLabel.Visible = false;

            _initialized = true;
        }
    }

    private void OnBodyEntered(Node body)
    {
        // Score for player if AI goal was entered, otherwise score for AI.
        // Regardless, reset the game state.
        GD.Print(body.Name);
        if (body.IsInGroup("goal"))
        {
            IncreaseScore(body.IsInGroup("goal_ai"));

            LinearVelocity = Vector2.Zero;
            PhysicsServer2D.BodySetState(GetRid(), PhysicsServer2D.BodyState.LinearVelocity, Vector2.Zero);

            GlobalPosition = _startingPos;
            PhysicsServer2D.BodySetState(GetRid(), PhysicsServer2D.BodyState.Transform,
                Transform2D.Identity.Translated(_startingPos));

            StartLabel.Visible = true;
            _initialized = false;
        }
    }

    private void IncreaseScore(bool playerScored)
    {
        if (playerScored)
        {
            _playerScore++;
        }
        else
        {
            _aiScore++;
        }

        ScoreLabel.Text = $"{_aiScore} - {_playerScore}";
    }
}
