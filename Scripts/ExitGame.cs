using Godot;
using System.Linq;

public partial class ExitGame : Label
{
    [Export] public Polygon2D Background;
    [Export] public float HoldDuration = 1.5f;

    private Color _startColor = new(0, 0, 0);
    private Color _endColor = new(0.5f, 0.5f, 0.5f);

    public override void _Ready()
    {
        var exitEvents =
            string.Join(", ", InputMap.ActionGetEvents("Exit").Select(inputEvent => inputEvent.AsText()));
        Text = $"Hold [{exitEvents}] to Exit";
    }

    public override void _Process(double delta)
    {
        float increase = 0;
        if (Input.IsActionPressed("Exit"))
        {
            increase = (float)delta / HoldDuration;
        }
        else if (Background.Color.ToHtml() != _startColor.ToHtml())
        {
            increase = -(float)delta / HoldDuration;
        }

        if (Background.Color.ToHtml() == _endColor.ToHtml())
        {
            GetTree().Quit();
        }
        else if (Mathf.Abs(increase) > 1e-6)
        {
            var weightRed = (Background.Color.R - _startColor.R) / (_endColor.R - _startColor.R) + increase;
            var red = Mathf.Lerp(_startColor.R, _endColor.R, weightRed);
            var weightGreen = (Background.Color.G - _startColor.G) / (_endColor.G - _startColor.G) + increase;
            var green = Mathf.Lerp(_startColor.G, _endColor.G, weightGreen);
            var weightBlue = (Background.Color.B - _startColor.B) / (_endColor.B - _startColor.B) + increase;
            var blue = Mathf.Lerp(_startColor.B, _endColor.B, weightBlue);

            Background.Color = new Color(red, green, blue);
        }
    }
}
