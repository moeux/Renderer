using Avalonia;
using Avalonia.Skia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class ModuloVisualizer : VisualizerBase
{
    private const int TileSize = 256;
    private SKPaint _paint;

    public ModuloVisualizer() : base("Modulo")
    {
        using var bitmap = new SKBitmap(TileSize, TileSize, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

        for (var y = 1; y < TileSize; y++)
        for (var x = 1; x < TileSize; x++)
            bitmap.SetPixel(x, y, new SKColor(
                (byte)(x % y),
                (byte)(x % y),
                (byte)(x % y),
                255));

        using var shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
        _paint = new SKPaint { IsAntialias = true, Shader = shader };
    }

    protected override void DrawFrame(SKCanvas canvas, Rect bounds)
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