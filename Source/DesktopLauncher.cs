using DesktopGLBackend.Core;

using LughSharp.Lugh.Core;
using LughSharp.Lugh.Utils;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public static class DesktopLauncher
{
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
            GLProfilingEnabled = false,
        };

        config.SetWindowedMode( 480, 320 );

        Gdx.GdxApi.CheckEnableDevMode().CheckEnableGodMode();

        var game = new DesktopGLApplication( new MainGame(), config );

        game.Run();
    }
}