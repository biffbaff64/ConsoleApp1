using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Utils;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    public InputMultiplexer? InputMultiplexer;
    public Keyboard?         Keyboard;

    // ------------------------------------------------------------------------
    
    private OrthographicCamera? _camera;
    private SpriteBatch?        _spriteBatch;
    private Texture?            _background;

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override void Create()
    {
        Logger.CheckPoint();

        _spriteBatch = new SpriteBatch();
        _camera      = new OrthographicCamera();
        _camera.SetToOrtho( false, Gdx.Graphics.Width, Gdx.Graphics.Height );
        _camera.Zoom = 0f;

        // --------------------------------------------------------------------
        // Working
//        var pm = new Pixmap( 100, 100, Pixmap.ColorFormat.RGBA8888 );
//        _background = new Texture( 100, 100, Pixmap.ColorFormat.RGBA8888 );

        // --------------------------------------------------------------------
        // Not Working
        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "red7logo_small.png" ).FileName ) );
        _background = new Texture( pm );

//        _background = new Texture( "red7logo_small.png" );

        Logger.Debug( $"pm.width: {pm.Width}, pm.height: {pm.Height}" );
        Logger.Debug( $"pm.Format: {pm.Format}" );
        Logger.Debug( $"pm.Pixels.BackingArray: {pm.Pixels?.BackingArray().Length}" );

        if ( _background != null )
        {
            Logger.Debug( $"_background: {_background.Width} x {_background.Height}" );
            Logger.Debug( $"_background Format: {_background.TextureData?.Format}" );
        }

        Logger.CheckPoint();
        
        Keyboard = new Keyboard();
        
        Logger.CheckPoint();

        InputMultiplexer = new InputMultiplexer();
        InputMultiplexer.AddProcessor( Keyboard );

        Logger.CheckPoint();

        Gdx.Input.InputProcessor = InputMultiplexer;
        
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

        if ( _camera != null && _spriteBatch != null )
        {
            _camera.Update();

            _spriteBatch.SetProjectionMatrix( _camera.Combined );
            _spriteBatch.Begin();

            if ( _background != null )
            {
                _spriteBatch.Draw( _background, 0, 0 );
            }

            _spriteBatch.End();
            _spriteBatch.EnableBlending();
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
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _background?.Dispose();
    }
}