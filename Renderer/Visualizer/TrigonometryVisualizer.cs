using System;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class TrigonometryVisualizer() : ShaderVisualizer("Trigonometry", CalculatePixel)
{
    private static SKColor CalculatePixel(int x, int y)
    {
        return new SKColor(
            (byte)(Math.Sin((double)x / 100) * 127 + 128),
            (byte)(Math.Cos((double)y / 100) * 127 + 128),
            (byte)(Math.Tan((double)x / 100) * 127 + 128),
            255);
    }
}