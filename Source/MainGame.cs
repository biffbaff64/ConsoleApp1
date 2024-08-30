using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
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
        _camera.SetToOrtho( false, 640, 480 );

        var pm = new Pixmap( 100, 100, Pixmap.Format.RGBA8888 );
        
        Logger.Debug( $"pm.Width: {pm.Width}, pm.Height: {pm.Height}, pm.Format: {pm.PixFormat}" );
        
//        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "LughLogo.png" ).FileName ) );
        _background = new Texture( pm );
//        _background = new Texture( "Felix.png" );
//        _background = new Texture( 100, 100, Pixmap.Format.RGBA8888 );
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue );

        _camera?.Update();

        _spriteBatch?.SetProjectionMatrix( _camera!.Combined );

        _spriteBatch?.Begin();

        if ( _background != null )
        {
            _spriteBatch?.Draw( _background!, 0, 0 );
        }
        
        _spriteBatch?.End();
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