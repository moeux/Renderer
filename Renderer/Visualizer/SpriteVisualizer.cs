using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Skia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class SpriteVisualizer : VisualizerBase
{
    private readonly SKImage _image;
    private readonly SKRect[] _sprites;
    private long _elapsed;
    private int _index;

    public SpriteVisualizer() : base("Sprite")
    {
        _image = SKImage.FromEncodedData(Path.GetFullPath("../../../../sprites/samurai.png"));
        var spriteSize = _image.Height;
        _sprites = Enumerable.Range(0, _image.Width / spriteSize - 1)
            .Select(i => SKRect.Create(i * spriteSize, 0, spriteSize, spriteSize))
            .ToArray();
    }

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        canvas.Clear(SKColors.LightSkyBlue);
        if (_elapsed >= TimeSpan.TicksPerSecond / 10)
        {
            _index = (_index + 1) % _sprites.Length;
            _elapsed = 0;
        }
        else
        {
            _elapsed += elapsedTicks;
        }

        canvas.DrawImage(
            _image,
            _sprites[_index],
            bounds.CenterRect(
                    _sprites[_index]
                        .ToAvaloniaRect()
                        .Inflate(300))
                .Translate(new Vector(-100, -100))
                .ToSKRect());
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _image.Dispose();
    }
}