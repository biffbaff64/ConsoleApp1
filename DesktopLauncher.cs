using ConsoleApp1.Source;
using LughSharp.Backends.DesktopGL;

namespace ConsoleApp1;

// ReSharper disable once MemberCanBeInternal
public static class DesktopLauncher
{
    public static void Main( string[] args )
    {
        var config = new DesktopGLApplicationConfiguration
        {
            Title         = "LughSharp Template",
            VSyncEnabled  = true,
            ForegroundFPS = 60,
            DisableAudio  = true
        };

        config.SetWindowedMode( 640, 480 );

        var game = new DesktopGLApplication( new MainGame(), config );
        game.Run();
    }
}

