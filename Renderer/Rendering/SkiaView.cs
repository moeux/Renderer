using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Renderer.Rendering;

public sealed class SkiaView : Control, IDisposable
{
    private int _index;
    private List<IVisualizer> Visualizers { get; set; } = [];

    public void Dispose()
    {
        Visualizers.ForEach(v => v.Dispose());
        Visualizers.Clear();
        Visualizers = null!;
    }

    public void AddVisualizer(params IVisualizer[] visualizers)
    {
        Visualizers.AddRange(visualizers);
    }

    public void NextVisualizer()
    {
        _index = _index + 1 >= Visualizers.Count ? 0 : Math.Min(Visualizers.Count - 1, _index + 1);
    }

    public void PreviousVisualizer()
    {
        _index = _index - 1 < 0 ? Visualizers.Count - 1 : Math.Max(0, _index - 1);
    }

    public override void Render(DrawingContext context)
    {
        if (_index >= Visualizers.Count) return;

        context.Custom(new SkiaDrawOperation(new Rect(0, 0, Bounds.Width, Bounds.Height), Visualizers[_index]));
        Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
    }
}