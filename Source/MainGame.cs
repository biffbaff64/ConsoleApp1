using System.Drawing;
using System.Drawing.Imaging;
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
    private Texture? _background;
    
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
//        _background = new Texture( 100, 100, Pixmap.ColorFormat.RGBA8888 );

        // --------------------------------------------------------------------
        // Not Working
        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "libgdx.png" ).FileName ) );
        _background = new Texture( pm );
//        _background = new Texture( "red7logo_small.png" );

        // --------------------------------------------------------------------
        // Initialise Input Multiplexer and Keyboard
//        Logger.Debug( "Setting up Keyboard" );
//        App.Keyboard         = new Keyboard();
//        App.InputMultiplexer = new InputMultiplexer();
//        App.InputMultiplexer.AddProcessor( App.Keyboard );
//        Gdx.Input.InputProcessor = App.InputMultiplexer;
        
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
        ScreenUtils.Clear( Color.Blue );

        if ( ( App.Camera != null ) && ( App.SpriteBatch != null ) )
        {
            App.Camera.Update();

            App.SpriteBatch.SetProjectionMatrix( App.Camera.Combined );
            App.SpriteBatch.Begin();

            if ( _background != null )
            {
                App.SpriteBatch.Draw( _background, 0, 0 );
            }

            App.SpriteBatch.End();
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
        _background?.Dispose();
    }
}