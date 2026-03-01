using System.IO;
using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class ImageVisualizer() : VisualizerBase("Image")
{
    private readonly SKImage _image = SKImage.FromEncodedData(Path.GetFullPath("../../../../img/banner.png"));

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        canvas.DrawImage(
            _image,
            (float)(bounds.Center.X - _image.Width / 2f),
            (float)bounds.Center.Y - _image.Height / 2f);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _image.Dispose();
    }
}