using DesktopGLBackend.Core;
using Corelib.LibCore.Core;
using Corelib.LibCore.Utils;

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

        if ( Environment.GetEnvironmentVariables().Contains( "DEV_MODE" ) )
        {
            Gdx.DevMode = Environment.GetEnvironmentVariable( "DEV_MODE" )!.Equals( "TRUE" );
        }
        
        Logger.Debug( $"Gdx.DevMode: {Gdx.DevMode}" );

        var game = new DesktopGLApplication( new MainGame(), config );
        
        game.Run();
    }
}