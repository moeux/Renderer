using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class BounceVisualizer() : VisualizerBase("Bounce")
{
    private const float Radius = 50;
    private const float Step = 0.5f;
    private float _force = 10;
    private float _horizontalRadius = Radius;
    private bool _isAscending;
    private float _modifier = 1;
    private SKPaint _paint = new() { Color = SKColors.Blue, IsAntialias = true, Style = SKPaintStyle.Fill };
    private float _verticalRadius = Radius;
    private float _y;

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        _y += _force;

        if (_force <= 0) _isAscending = false;

        if (_y + Radius >= bounds.Bottom)
        {
            _force = 0;
            _verticalRadius -= Step * _modifier;
            _horizontalRadius += Step * _modifier;

            if (_verticalRadius <= 5) _modifier *= -1;
            if (_horizontalRadius >= 55)
            {
                _isAscending = true;
                _force = -10;
            }

            canvas.DrawOval((float)bounds.Center.X, _y, _verticalRadius, _horizontalRadius, _paint);
        }
        else
        {
            canvas.DrawCircle((float)bounds.Center.X, _y, Radius, _paint);
        }

        if (_isAscending)
            _force += 0.05f;
        else
            _force -= 0.05f;
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _paint.Dispose();
        _paint = null!;
    }
}