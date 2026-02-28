using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class ArithmeticVisualizer() : ShaderVisualizer("Arithmetic", CalculatePixel)
{
    private static SKColor CalculatePixel(int x, int y)
    {
        return new SKColor(
            (byte)(x % y),
            (byte)(x + y),
            (byte)(x - y),
            255);
    }
}