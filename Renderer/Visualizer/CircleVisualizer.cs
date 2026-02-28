using System;
using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class CircleVisualizer() : VisualizerBase("Circle")
{
    private const int Speed = 300;
    private SKPaint _paint = new() { Color = SKColors.Red, IsAntialias = true };
    private int _radius = 50;
    private double _x;

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        var elapsedTime = (double)elapsedTicks / TimeSpan.TicksPerSecond;
        _x += Speed * elapsedTime;
        _radius += _radius switch
        {
            <= 10 => 1,
            >= 50 => -1,
            _ => 1
        };

        if (_x + _radius >= bounds.Right)
            canvas.DrawCircle(
                (float)(bounds.Left + _x - bounds.Right),
                (float)bounds.Center.Y,
                _radius,
                _paint);

        if (_x - _radius >= bounds.Right) _x = (float)(bounds.Left + _radius);

        canvas.DrawCircle((float)_x, (float)bounds.Center.Y, _radius, _paint);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _paint.Dispose();
        _paint = null!;
    }
}