using ConsoleApp1.Source;
using LughSharp.Backends.DesktopGL;
using LughSharp.LibCore.Core;

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

        config.EnableGLDebugOutput( true, new StreamWriter( "GLDebug.log" ) );
        config.SetWindowedMode( 640, 480 );

        var game = new DesktopGLApplication( new MainGame(), config );

        Gdx.DevMode = true;
        
        game.Run();
    }
}