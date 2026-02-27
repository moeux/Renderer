using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class CircleVisualizer() : VisualizerBase("Circle")
{
    private const int Step = 5;
    private SKPaint _paint = new() { Color = SKColors.Red, IsAntialias = true };
    private int _radius = 50;
    private int _radiusStep = 1;
    private float _x;

    protected override void DrawFrame(SKCanvas canvas, Rect bounds)
    {
        _x += Step;
        _radiusStep = _radius switch
        {
            <= 10 => 1,
            >= 50 => -1,
            _ => _radiusStep
        };
        _radius += _radiusStep;

        if (_x + _radius >= bounds.Right)
            canvas.DrawCircle(
                (float)(bounds.Left + _x - bounds.Right),
                (float)bounds.Center.Y,
                _radius,
                _paint);

        if (_x - _radius >= bounds.Right) _x = (float)(bounds.Left + _radius);

        canvas.DrawCircle(_x, (float)bounds.Center.Y, _radius, _paint);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _paint.Dispose();
        _paint = null!;
    }
}