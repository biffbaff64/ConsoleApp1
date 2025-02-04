//#define KEYBOARD

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.GLUtils;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;
    private const string TEST_ASSET2 = Assets.LIBGDX_LOGO;

    private const int X = 40;
    private const int Y = 40;

    private AssetManager? _assetManager;
    private Texture?      _image1;
    private Texture?      _image2;

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        App.SpriteBatch = new SpriteBatch();
        App.Camera = new OrthographicCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height )
        {
            Zoom = 1f,
        };

        _assetManager = new AssetManager();
        _image1   = null;
        _image2        = null;

        // ====================================================================
        // ====================================================================

#if KEYBOARD
        Logger.Debug( "Setting up Keyboard" );
        App.Keyboard = new Keyboard();
        App.InputMultiplexer = new InputMultiplexer();
        App.InputMultiplexer.AddProcessor( App.Keyboard );
        GdxApi.Input.InputProcessor = App.InputMultiplexer;
#endif

        LoadAssets();

        Logger.Debug( "Done" );
    }

    /// <inheritdoc />
    public override void Update()
    {
    }

    private bool _canRender = true;

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue );

        if ( _canRender && ( App.Camera != null ) && ( App.SpriteBatch != null ) )
        {
            App.Camera.Update();

            App.SpriteBatch.SetProjectionMatrix( App.Camera.Combined );
            App.SpriteBatch.SetTransformMatrix( App.Camera.View );
            App.SpriteBatch.DisableBlending();
            App.SpriteBatch.Begin();

            if ( _image1 != null )
            {
                App.SpriteBatch.Draw( _image1, X, Y, _image1.Width, _image1.Height );
            }

            if ( _image2 != null )
            {
                App.SpriteBatch.Draw( _image2, X, Y, _image2.Width, _image2.Height );
            }

            App.SpriteBatch.End();

            _canRender = false;
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

        _image1?.Dispose();
        _image2?.Dispose();
        _assetManager?.Dispose();

        GC.SuppressFinalize( this );
    }

    // ========================================================================
    // ========================================================================

    private void LoadAssets()
    {
        GdxRuntimeException.ThrowIfNull( _assetManager );

        Logger.Divider();
        Logger.Debug( "Loading assets...", true );
        Logger.Divider();

        _assetManager.Load( TEST_ASSET1, typeof( Texture ), new TextureLoader.TextureLoaderParameters() );
//        _assetManager.FinishLoadingAsset( TEST_ASSET1 );

//        _assetManager.Load( TEST_ASSET2, typeof( Texture ), new TextureLoader.TextureLoaderParameters() );
//        _assetManager.FinishLoading();

//        if ( _assetManager.Contains( TEST_ASSET1 ) )
//        {
//            _image1 = _assetManager.GetTexture( TEST_ASSET1 );
//        }

//        if ( _assetManager.Contains( TEST_ASSET2 ) )
//        {
//            _image2 = _assetManager.GetTexture( TEST_ASSET2 );
//        }

//        if ( ( _image1 != null ) && ( _image2 != null ) )
//        {
//            Logger.Debug( "Asset loading failed" );
//        }
    }

    // ========================================================================
    // ========================================================================
}