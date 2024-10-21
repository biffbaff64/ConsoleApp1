using DesktopGLBackend.Core;
using Corelib.LibCore.Core;

namespace ConsoleApp1.Source;

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
        config.SetWindowedMode( 480, 320 );

        var game = new DesktopGLApplication( new MainGame(), config );

        Gdx.DevMode = true;
        
        game.Run();
    }
}