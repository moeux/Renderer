using System;
using Avalonia;
using Avalonia.Skia;
using SkiaSharp;

namespace Renderer.Rendering;

public abstract class ShaderVisualizer : VisualizerBase
{
    private const int TileSize = 256;
    private SKPaint _paint;

    protected ShaderVisualizer(string name, Func<int, int, SKColor> calculatePixel) : base(name)
    {
        using var bitmap = new SKBitmap(TileSize, TileSize, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

        for (var y = 1; y < TileSize; y++)
        for (var x = 1; x < TileSize; x++)
            bitmap.SetPixel(x, y, calculatePixel(x, y));

        using var shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
        _paint = new SKPaint { IsAntialias = true, Shader = shader };
    }

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        canvas.DrawRect(bounds.ToSKRect(), _paint);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _paint.Dispose();
        _paint = null!;
    }
}