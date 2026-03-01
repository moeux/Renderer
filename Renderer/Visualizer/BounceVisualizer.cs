using System;
using Avalonia;
using Renderer.Rendering;
using SkiaSharp;

namespace Renderer.Visualizer;

public class BounceVisualizer() : VisualizerBase("Bounce")
{
    private const float Radius = 50f;
    private const float InitialFallSpeed = 10f;
    private const float Gravity = 0.05f;
    private const float BounceDamping = 0.82f;
    private const float MinBounceSpeed = 1f;
    private const float DeformStep = 0.5f;
    private const float MinRadiusY = 5f;
    private const float MaxRadiusX = 55f;
    private const long RestartDelayTicks = TimeSpan.TicksPerSecond;

    private float _bounceSpeed = InitialFallSpeed;
    private bool _isGrounded;
    private bool _isResting;
    private SKPaint _paint = new() { Color = SKColors.Blue, IsAntialias = true, Style = SKPaintStyle.Fill };
    private float _radiusX = Radius;
    private float _radiusY = Radius;
    private long _restTicks;
    private float _squashDirection = 1f;
    private float _velocityY = InitialFallSpeed;
    private float _y = Radius;

    protected override void DrawFrame(SKCanvas canvas, Rect bounds, long elapsedTicks)
    {
        var centerX = (float)bounds.Center.X;
        var floorY = (float)bounds.Bottom - Radius;

        if (_isResting)
        {
            UpdateResting(elapsedTicks, floorY);
            canvas.DrawCircle(centerX, _y, Radius, _paint);
            return;
        }

        _velocityY += Gravity;
        _y += _velocityY;

        if (_y >= floorY)
        {
            HandleGroundContact(floorY);
            canvas.DrawOval(centerX, _y, _radiusY, _radiusX, _paint);
            return;
        }

        _isGrounded = false;
        canvas.DrawCircle(centerX, _y, Radius, _paint);
    }

    private void HandleGroundContact(float floorY)
    {
        _y = floorY;
        _velocityY = 0;

        _radiusY -= DeformStep * _squashDirection;
        _radiusX += DeformStep * _squashDirection;

        if (_radiusY <= MinRadiusY) _squashDirection = -1f;

        if (!(_radiusX >= MaxRadiusX)) return;

        if (!_isGrounded) TryReboundOrRest();

        _isGrounded = true;
        ResetShape();
    }

    private void TryReboundOrRest()
    {
        _bounceSpeed *= BounceDamping;

        if (_bounceSpeed < MinBounceSpeed)
        {
            _isResting = true;
            _restTicks = 0;
            _velocityY = 0;
            return;
        }

        _velocityY = -_bounceSpeed;
    }

    private void UpdateResting(long elapsedTicks, float floorY)
    {
        _y = floorY;
        _restTicks += elapsedTicks;

        if (_restTicks >= RestartDelayTicks) ResetCycle();
    }

    private void ResetCycle()
    {
        _isGrounded = false;
        _isResting = false;
        _restTicks = 0;
        _bounceSpeed = InitialFallSpeed;
        _velocityY = InitialFallSpeed;
        _y = Radius;
        ResetShape();
    }

    private void ResetShape()
    {
        _radiusY = Radius;
        _radiusX = Radius;
        _squashDirection = 1f;
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;

        _paint.Dispose();
        _paint = null!;
    }
}