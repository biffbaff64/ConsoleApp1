﻿using DesktopGLBackend.Core;

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
            Title         = "LughSharp Template",
            VSyncEnabled  = true,
            ForegroundFPS = 60,
            DisableAudio  = true,
        };

//        config.EnableGLDebugOutput( true, new StreamWriter( "GLDebug.log" ) );
        config.Debug = true;
        config.SetWindowedMode( 480, 320 );

        Gdx.GdxApi
           .CheckEnableDevMode()
           .CheckEnableGodMode();

        Logger.Debug( $"DevMode: {Gdx.GdxApi.DevMode}" );

        var game = new DesktopGLApplication( new MainGame(), config );

        game.Run();
    }
}