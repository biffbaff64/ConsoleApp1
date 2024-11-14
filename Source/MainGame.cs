using Corelib.LibCore.Assets;
using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.Cameras;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Utils;
using Color = Corelib.LibCore.Graphics.Color;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    private const int X = 0;
    private const int Y = 0;

    private readonly AssetManager _assetManager = new();
    private readonly Texture?     _background   = null;
    private          Texture?     _image        = null;

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        Logger.Checkpoint();

        App.SpriteBatch = new SpriteBatch();
        App.Camera = new OrthographicCamera( Gdx.Graphics.Width, Gdx.Graphics.Height )
        {
            Zoom = 0f,
        };

        Logger.Debug( "Camera:" );
        Logger.Debug( $"Width: {App.Camera.ViewportWidth}, Height: {App.Camera.ViewportHeight}" );
        Logger.Debug( $"X: {App.Camera.Position.X}, Y: {App.Camera.Position.Y}, Z: {App.Camera.Position.Z}" );

        // ====================================================================
        // ====================================================================

//        var pm = new Pixmap( 100, 100, Pixmap.ColorFormat.RGBA8888 );
//        _background = new Texture( pm, pm.Format );

//        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "libgdx.png" ).FileName ) );
//        _background = new Texture( pm );

//        _background = new Texture( "libgdx.png" );

        LoadAssets();

        // ====================================================================
        // ====================================================================

        Logger.Debug( "Setting up Keyboard" );

        App.Keyboard         = new Keyboard();
        App.InputMultiplexer = new InputMultiplexer();
        App.InputMultiplexer.AddProcessor( App.Keyboard );
        Gdx.Input.InputProcessor = App.InputMultiplexer;
        
        Logger.Debug( "Done" );
    }

    /// <inheritdoc />
    public override void Update()
    {
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Gdx.Graphics.WindowBackgroundColor );

        if ( ( App.Camera != null ) && ( App.SpriteBatch != null ) )
        {
            App.Camera.Update();

            App.SpriteBatch.SetProjectionMatrix( App.Camera.Combined );
            App.SpriteBatch.Begin();

            if ( _background != null )
            {
                App.SpriteBatch.Draw( _background, X, Y, _background.Width, _background.Height );
            }

            if ( _image != null )
            {
                App.SpriteBatch.Draw( _image, X, Y, _image.Width, _image.Height );
            }

            App.SpriteBatch.End();
        }
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
        _image?.Dispose();
        _assetManager.Dispose();
        
        GC.SuppressFinalize( this );
    }

    // ========================================================================
    // ========================================================================

    private void LoadAssets()
    {
        Logger.Checkpoint();
        Logger.Divider();
        Logger.Debug( "Loading assets...", true );
        Logger.Divider();

        _assetManager.AddToLoadqueue( Assets.ROVER_WHEEL, typeof( Texture ) );

        Logger.Debug( "All assets queued for loading.", true );

        _assetManager.DisplayMetrics();
        _assetManager.FinishLoading();
        _assetManager.DisplayMetrics();

        _image ??= _assetManager.Get( Assets.ROVER_WHEEL ) as Texture;
    }

    // ========================================================================
    // ========================================================================
}