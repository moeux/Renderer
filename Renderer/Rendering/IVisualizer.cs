using System;
using Avalonia;
using SkiaSharp;

namespace Renderer.Rendering;

public interface IVisualizer : IDisposable
{
    public void Draw(SKCanvas canvas, Rect bounds);
}