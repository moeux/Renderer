using System;
using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class PongVisualizer() : VisualizerBase("Pong")
{
    private const int Radius = 5;
    private const int PaddleWidth = 10;
    private const int PaddleHeight = 100;
    private const float Speed = 1 + 1e-3f * 3;
    private readonly SKRoundRect _leftPaddle = new(SKRect.Empty, Radius);
    private readonly SKPaint _paint = new() { Color = SKColors.Black, IsAntialias = true, TextSize = 48 };
    private readonly SKRoundRect _rightPaddle = new(SKRect.Empty, Radius);
    private SKPoint _ballPosition = SKPoint.Empty;
    private long _elapsedTicks;
    private SKPoint _velocity = SKPoint.Empty;

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        if (_ballPosition == SKPoint.Empty)
            _ballPosition = new SKPoint((float)bounds.Center.X, (float)bounds.Center.Y);

        if (_velocity == SKPoint.Empty)
            _velocity = new SKPoint(Random.Shared.NextSingle() + .1f, Random.Shared.NextSingle() + .1f);

        if (_ballPosition.X + Radius > bounds.Right)
        {
            canvas.DrawText(
                "Player 1 Wins!",
                (float)bounds.Center.X - _paint.MeasureText("Player 1 Wins!") / 2,
                (float)bounds.Center.Y + _paint.TextSize / 2, _paint);

            _elapsedTicks += elapsedTicks;

            if (_elapsedTicks < TimeSpan.TicksPerSecond * 3) return;

            _elapsedTicks = 0;
            _ballPosition = new SKPoint((float)bounds.Center.X, (float)bounds.Center.Y);
            _velocity = new SKPoint(Random.Shared.NextSingle() + .1f, Random.Shared.NextSingle() + .1f);

            return;
        }

        if (_ballPosition.X - Radius < bounds.Left)
        {
            canvas.DrawText(
                "Player 2 Wins!",
                (float)bounds.Center.X - _paint.MeasureText("Player 2 Wins!") / 2,
                (float)bounds.Center.Y + _paint.TextSize / 2, _paint);

            _elapsedTicks += elapsedTicks;

            if (_elapsedTicks < TimeSpan.TicksPerSecond * 3) return;

            _elapsedTicks = 0;
            _ballPosition = new SKPoint((float)bounds.Center.X, (float)bounds.Center.Y);
            _velocity = new SKPoint(Random.Shared.NextSingle() + .1f, Random.Shared.NextSingle() + .1f);
            return;
        }

        _velocity = new SKPoint(_velocity.X * Speed, _velocity.Y * Speed);

        if (_ballPosition.Y + Radius >= bounds.Bottom || _ballPosition.Y - Radius <= bounds.Top)
            _velocity = new SKPoint(_velocity.X, -_velocity.Y);
        if (_leftPaddle.Rect.Contains(_ballPosition) || _rightPaddle.Rect.Contains(_ballPosition))
            _velocity = new SKPoint(-_velocity.X, _velocity.Y);

        _leftPaddle.SetRectRadii(
            SKRect.Create(
                (float)(bounds.Left + Radius),
                (float)Math.Min(
                    bounds.Bottom - PaddleHeight,
                    Math.Max(bounds.Top, _ballPosition.Y - PaddleHeight / 2f)),
                PaddleWidth,
                PaddleHeight),
            [
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius)
            ]);
        _rightPaddle.SetRectRadii(
            SKRect.Create(
                (float)(bounds.Right - Radius - PaddleWidth),
                (float)Math.Min(
                    bounds.Bottom - PaddleHeight,
                    Math.Max(bounds.Top, _ballPosition.Y - PaddleHeight / 2f)),
                PaddleWidth,
                PaddleHeight),
            [
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius),
                new SKPoint(Radius, Radius)
            ]);
        _ballPosition.Offset(_velocity.X, _velocity.Y);

        canvas.DrawRoundRect(_leftPaddle, _paint);
        canvas.DrawRoundRect(_rightPaddle, _paint);
        canvas.DrawCircle(_ballPosition, Radius, _paint);
    }

    protected override void Dispose(bool disposing)
    {
        _leftPaddle.Dispose();
        _rightPaddle.Dispose();
        _paint.Dispose();
    }
}