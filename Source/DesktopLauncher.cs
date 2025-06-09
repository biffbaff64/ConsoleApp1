using DesktopGLBackend.Core;

using LughSharp.Lugh.Core;

namespace ConsoleApp1.Source;

/// <summary>
/// Example DesktopLauncher class which is the entry point for the
/// desktop application.
/// </summary>
public static class DesktopLauncher
{
    /// <summary>
    /// Entry point for the desktop application. Initializes the application configuration
    /// and launches the main game loop.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    [STAThread]
    private static void Main( string[] args )
    {
        var config = new DesktopGLApplicationConfiguration
        {
            Title              = "LughSharp Template",
            VSyncEnabled       = true,
            ForegroundFPS      = 60,
            DisableAudio       = true,
            Debug              = true,
            GLProfilingEnabled = true,
            WindowWidth        = 480,
            WindowHeight       = 320,
        };

        Engine.Api.CheckEnableDevMode().CheckEnableGodMode();

        var game = new DesktopGLApplication( new MainGame(), config );

        game.Run();
    }
}