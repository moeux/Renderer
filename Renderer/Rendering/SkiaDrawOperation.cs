using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Renderer.Rendering;

public class SkiaDrawOperation : ICustomDrawOperation
{
    private IVisualizer _visualizer;

    public SkiaDrawOperation(Rect bounds, IVisualizer visualizer)
    {
        Bounds = bounds;
        _visualizer = visualizer;
    }

    public Rect Bounds { get; }

    public bool HitTest(Point p)
    {
        return Bounds.Contains(p);
    }

    public void Dispose()
    {
        _visualizer = null!;
        GC.SuppressFinalize(this);
    }

    public bool Equals(ICustomDrawOperation? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is SkiaDrawOperation op) return op.Bounds == Bounds && op._visualizer == _visualizer;
        return false;
    }

    public void Render(ImmediateDrawingContext context)
    {
        if (!context.TryGetFeature<ISkiaSharpApiLeaseFeature>(out var skia))
            return;

        using var lease = skia.Lease();
        var canvas = lease.SkCanvas;
        var count = canvas.Save();

        canvas.ClipRect(Bounds.ToSKRect());
        canvas.Clear(SKColors.White);
        _visualizer.Draw(canvas, new Rect(0, 0, Bounds.Width, Bounds.Height));
        canvas.RestoreToCount(count);
    }
}