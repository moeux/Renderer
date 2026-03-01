using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Renderer.Rendering;
using Renderer.Visualizer;

namespace Renderer;

public partial class MainWindow : Window
{
    private SkiaView? _surface;

    public MainWindow()
    {
        InitializeComponent();

        Closed += OnClosed;

        _surface = this.FindControl<SkiaView>("Surface");

        _surface?.AddVisualizer(
            new ArithmeticVisualizer(),
            new BounceVisualizer(),
            new CircleVisualizer(),
            new ModuloVisualizer(),
            new PongVisualizer(),
            new TrigonometryVisualizer());
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        _surface?.Dispose();
        _surface = null;
        Closed -= OnClosed;
    }

    private void PreviousVisualizerClick(object? sender, RoutedEventArgs e)
    {
        _surface?.PreviousVisualizer();
    }

    private void NextVisualizerClick(object? sender, RoutedEventArgs e)
    {
        _surface?.NextVisualizer();
    }
}