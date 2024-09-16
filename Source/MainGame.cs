using System.Drawing;
using System.Text.RegularExpressions;
using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Utils;
using Color = LughSharp.LibCore.Graphics.Color;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    /// <inheritdoc />
    public override void Create()
    {
        Logger.CheckPoint();

        App.SpriteBatch = new SpriteBatch();
        App.Camera      = new OrthographicCamera();
        App.Camera.SetToOrtho( false, Gdx.Graphics.Width, Gdx.Graphics.Height );
        App.Camera.Zoom = 0f;

        // --------------------------------------------------------------------
        // Working
//        var pm = new Pixmap( 100, 100, Pixmap.ColorFormat.RGBA8888 );
//        App.Background = new Texture( 100, 100, Pixmap.ColorFormat.RGBA8888 );

        // --------------------------------------------------------------------
        // Not Working
//        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "red7logo_small.png" ).FileName ) );
//        App.Background = new Texture( pm );

        App.Background = new Texture( "red7logo_small.png" );

//        Logger.Debug( $"pm.Width: {pm.Width}, pm.Height: {pm.Height}" );
//        Logger.Debug( $"pm.Format: {pm.Format}" );
//        Logger.Debug( $"pm.Pixels.BackingArray: {pm.Pixels?.BackingArray().Length}" );

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if ( App.Background != null )
        {
            Logger.Debug( $"Background       : {App.Background.Width} x {App.Background.Height}" );
            Logger.Debug( $"Background Format: {App.Background.TextureData?.Format}" );
        }

        Logger.CheckPoint( true );
        Logger.Debug( "Setting up Keyboard" );

        App.Keyboard         = new Keyboard();
        App.InputMultiplexer = new InputMultiplexer();
        App.InputMultiplexer.AddProcessor( App.Keyboard );
        Gdx.Input.InputProcessor = App.InputMultiplexer;

//        App.Font = new BitmapFont();

        Logger.CheckPoint();
    }

//    /// <inheritdoc />
//    public override void Update()
//    {
//        base.Update();
//    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue, true );

        if ( ( App.Camera != null ) && ( App.SpriteBatch != null ) )
        {
            App.Camera.Update();

            App.SpriteBatch.SetProjectionMatrix( App.Camera.Combined );
            App.SpriteBatch.Begin();

            if ( App.Background != null )
            {
                App.SpriteBatch.Draw( App.Background, 0, 0 );
            }

//            App.Font?.Draw( App.SpriteBatch, "TEST", 0, 0 );

            App.SpriteBatch.End();
            App.SpriteBatch.EnableBlending();
        }
    }

    /// <inheritdoc />
    public override void Pause()
    {
    }

    /// <inheritdoc />
    public override void Resume()
    {
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        if ( App.Camera != null )
        {
            App.Camera.ViewportWidth  = width;
            App.Camera.ViewportHeight = height;
            App.Camera.Update();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        App.SpriteBatch?.Dispose();
        App.Background?.Dispose();
    }
}