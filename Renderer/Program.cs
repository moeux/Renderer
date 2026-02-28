using System;
using Avalonia;
using Avalonia.X11;

namespace Renderer;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UseSkia()
            .UsePlatformDetect()
            .With(new X11PlatformOptions
            {
                UseDBusMenu = false,
                UseDBusFilePicker = false
            })
            .WithInterFont()
            .LogToTrace();
    }
}
