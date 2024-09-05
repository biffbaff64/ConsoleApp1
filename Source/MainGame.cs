using System.Globalization;
using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
using LughSharp.LibCore.Graphics.FrameBuffers;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Utils;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    private OrthographicCamera? _camera;
    private SpriteBatch?        _spriteBatch;
    private Texture?            _background;

    /// <inheritdoc />
    public override void Create()
    {
        Logger.CheckPoint();

        _spriteBatch = new SpriteBatch();
        _camera      = new OrthographicCamera();
        _camera.SetToOrtho( false, Gdx.Graphics.Width, Gdx.Graphics.Height );
        _camera.Zoom = 4.0f;
        
        // --------------------------------------------------------------------
        // Working
//        var pm = new Pixmap( 100, 100, Pixmap.Format.RGBA8888 );
//        _background = new Texture( 100, 100, Pixmap.Format.RGBA8888 );

        // --------------------------------------------------------------------
        // Not Working
//        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "red7logo_small.png" ).FileName ) );
//        _background = new Texture( pm );
        
        _background = new Texture( "red7logo_small.png" ); // Creates, and displays, window. Doesn't draw image.
        
        Logger.Debug( $"_background: {_background.Width}x{_background.Height}" );
        Logger.Debug( $"_background Format: {_background.TextureData?.Format}" );
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Black );

        _camera?.Update();

        _spriteBatch?.SetProjectionMatrix( _camera!.Combined );
        _spriteBatch?.DisableBlending();
        _spriteBatch?.Begin();

        if ( _background != null )
        {
            _spriteBatch?.Draw( _background!, 0, 0 );
        }

        _spriteBatch?.End();
        _spriteBatch?.EnableBlending();
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
    }
}