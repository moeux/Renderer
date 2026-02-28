using System;
using System.Diagnostics;
using Avalonia;
using SkiaSharp;

namespace Renderer.Rendering;

public abstract class VisualizerBase(string name) : IVisualizer
{
    private SKPaint _backgroundPaint = new() { Color = SKColors.DarkGray, IsAntialias = true };
    private double _fps;
    private int _frameCount;
    private long _lastFpsUpdate;
    private long _lastUpdate;
    private SKPaint _paint = new() { Color = SKColors.Lime, IsAntialias = true, TextSize = 24 };
    private Stopwatch _stopwatch = Stopwatch.StartNew();

    public void Draw(SKCanvas canvas, Rect bounds)
    {
        CalculateFps();
        DrawFrame(canvas, bounds, _stopwatch.Elapsed.Ticks - _lastUpdate);
        DrawUi(canvas, bounds);

        _lastUpdate = _stopwatch.Elapsed.Ticks;
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        _stopwatch = null!;
        _paint.Dispose();
        _paint = null!;
        _backgroundPaint.Dispose();
        _backgroundPaint = null!;
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void DrawUi(SKCanvas canvas, Rect bounds)
    {
        var x = (float)bounds.Right - _paint.MeasureText(name);
        var height = _paint.TextSize + 10;
        var fpsTextWidth = _paint.MeasureText($"{_fps:0.0}");
        var nameTextWidth = _paint.MeasureText(name);

        canvas.DrawRoundRect(5, 5, fpsTextWidth + 10, height, 5, 5, _backgroundPaint);
        canvas.DrawText($"{_fps:0.0}", 10, 30, _paint);
        canvas.DrawRoundRect(x - 15, 5, nameTextWidth + 10, height, 5, 5, _backgroundPaint);
        canvas.DrawText($"{name}", x - 10, 30, _paint);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    private void CalculateFps()
    {
        var now = _stopwatch.Elapsed.Ticks;
        var elapsed = now - _lastFpsUpdate;
        _frameCount++;

        if (!(elapsed >= 0.5 * TimeSpan.TicksPerSecond)) return;

        _fps = _frameCount / ((double)elapsed / TimeSpan.TicksPerSecond);
        _frameCount = 0;
        _lastFpsUpdate = now;
    }

    protected abstract void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks);
}