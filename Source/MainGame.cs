//#define KEYBOARD

using System.Diagnostics.CodeAnalysis;

using Corelib.Lugh.Assets;
using Corelib.Lugh.Core;
using Corelib.Lugh.Graphics.Cameras;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Utils;

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
        Logger.Debug( $"GdxApi.Graphics       : {Gdx.GdxApi.Graphics}" );
        Logger.Debug( $"GdxApi.Graphics.Width : {Gdx.GdxApi.Graphics.Width}" );
        Logger.Debug( $"GdxApi.Graphics.Height: {Gdx.GdxApi.Graphics.Height}" );
        
        App.SpriteBatch = new SpriteBatch();
        Logger.Checkpoint();
        App.Camera = new OrthographicCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height )
        {
            Zoom = 0f,
        };
        Logger.Checkpoint();

        Logger.Debug( "Camera:" );
        Logger.Debug( $"Width: {App.Camera.ViewportWidth}, Height: {App.Camera.ViewportHeight}" );
        Logger.Debug( $"X: {App.Camera.Position.X}, Y: {App.Camera.Position.Y}, Z: {App.Camera.Position.Z}" );

        // ====================================================================
        // ====================================================================

#if KEYBOARD
        Logger.Debug( "Setting up Keyboard" );
        App.Keyboard = new Keyboard();
        App.InputMultiplexer = new InputMultiplexer();
        App.InputMultiplexer.AddProcessor( App.Keyboard );
        GdxApi.Input.InputProcessor = App.InputMultiplexer;
#endif

        // ====================================================================
        // ====================================================================

        LoadAssets();

        Logger.Debug( "Done" );
    }

    /// <inheritdoc />
    public override void Update()
    {
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Gdx.GdxApi.Graphics.WindowBackgroundColor );

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

    [SuppressMessage( "Interoperability", "CA1416:Validate platform compatibility" )]
    private void LoadAssets()
    {
        Logger.Divider();
        Logger.Debug( "Loading assets...", true );
        Logger.Divider();

        _assetManager.AddToLoadqueue( Assets.ROVER_WHEEL, typeof( Texture ) );
        _assetManager.FinishLoading();

        _image ??= _assetManager.Get( Assets.ROVER_WHEEL ) as Texture;
    }

    // ========================================================================
    // ========================================================================
}