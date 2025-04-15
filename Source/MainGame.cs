//#define KEYBOARD
//#define OGL_TEST
#define JSON_TEST
//#define PACK_IMAGES
//define LOAD_ASSETS

// ============================================================================

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;
using LughSharp.Tests.Source;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;
    private const string TEST_ASSET2 = Assets.LIBGDX_LOGO;
    private const string TEST_ASSET3 = Assets.RED7_LOGO;

    private const int X = 40;
    private const int Y = 40;

    private AssetManager? _assetManager;
    private Texture?      _image1;
    private Texture?      _image2;

    private Texture? _image3;

//    private BitmapFont?         _bitmapFont;
    private OrthographicCamera? _camera;
    private SpriteBatch?        _spriteBatch;

    #if OGL_TEST
    private readonly OpenGLTest _openGLTest = new();
    #endif

    #if JSON_TEST
    private readonly JsonTest _jsonTest = new();
    #endif
    
    #if PACK_IMAGES
    private const bool REBUILD_ATLAS           = true;
    private const bool REMOVE_DUPLICATE_IMAGES = true;
    private const bool DRAW_DEBUG_LINES        = false;
    #endif
    
    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        _spriteBatch = new SpriteBatch();

        _camera = new OrthographicCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height )
        {
            Zoom = 1f,
        };

        _assetManager = new AssetManager();
        _image1       = null;
        _image2       = null;

//        _bitmapFont   = new BitmapFont();

        // ====================================================================
        // ====================================================================

        #if KEYBOARD
        Logger.Debug( "Setting up Keyboard" );
        _keyboard = new Keyboard();
        _inputMultiplexer = new InputMultiplexer();
        _inputMultiplexer.AddProcessor( _keyboard );
        GdxApi.Input.InputProcessor = _inputMultiplexer;
        #endif

        #if PACK_IMAGES
        PackImages();
        #endif
        
        #if LOAD_ASSETS
        LoadAssets();
        #endif

        #if OGL_TEST
        _openGLTest.Create();
        #endif

        #if JSON_TEST
        _jsonTest.Create();
        #endif
        
        Logger.Debug( "Done" );
    }

    /// <inheritdoc />
    public override void Update()
    {
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue );

        if ( ( _camera != null ) && ( _spriteBatch != null ) )
        {
            _camera.Update();

            _spriteBatch.SetProjectionMatrix( _camera.Combined );
            _spriteBatch.SetTransformMatrix( _camera.View );

//            _spriteBatch.DisableBlending();
            _spriteBatch.Begin();

            if ( _image1 != null )
            {
                _spriteBatch.Draw( _image1, X, Y, _image1.Width, _image1.Height );
            }

            if ( _image2 != null )
            {
                _spriteBatch.Draw( _image2, X, Y, _image2.Width, _image2.Height );
            }

            if ( _image3 != null )
            {
                _spriteBatch.Draw( _image3, X, Y, _image3.Width, _image3.Height );
            }

            #if OGL_TEST
            _openGLTest.Render();
            #endif

            _spriteBatch.End();
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        if ( _camera != null )
        {
            _camera.ViewportWidth  = width;
            _camera.ViewportHeight = height;
            _camera.Update();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();

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

//        _assetManager.Load( TEST_ASSET2, typeof( Texture ), new TextureLoader.TextureLoaderParameters() );
//        _assetManager.Load( TEST_ASSET3, typeof( Texture ), new TextureLoader.TextureLoaderParameters() );
        _assetManager.FinishLoading();

        if ( _assetManager.Contains( TEST_ASSET1 ) )
        {
            _image1 = _assetManager.GetTexture( TEST_ASSET1 );
            Logger.Debug( "_image1 is SET" );
        }

        if ( _assetManager.Contains( TEST_ASSET2 ) )
        {
            _image2 = _assetManager.GetTexture( TEST_ASSET2 );
            Logger.Debug( "_image2 is SET" );
        }

        if ( _assetManager.Contains( TEST_ASSET3 ) )
        {
            _image3 = _assetManager.GetTexture( TEST_ASSET3 );
            Logger.Debug( "_image3 is SET" );
        }

//        if ( ( _image1 != null ) && ( _image2 != null ) && ( _image3 != null ) )
//        {
//            Logger.Debug( "Asset loading failed" );
//        }
    }

    // ========================================================================
    // ========================================================================

    #if PACK_IMAGES
    [SupportedOSPlatform( "windows" )]
    private static void PackImages()
    {
        if ( REBUILD_ATLAS )
        {
            var settings = new TexturePacker.Settings
            {
                MaxWidth   = 2048, // Maximum Width of final atlas image
                MaxHeight  = 2048, // Maximum Height of final atlas image
                PowerOfTwo = true,
                Debug      = DRAW_DEBUG_LINES,
                IsAlias    = REMOVE_DUPLICATE_IMAGES,
            };

            // Build the Atlases from the specified parameters :-
            // - configuration settings
            // - source folder
            // - destination folder
            // - name of atlas, without extension (the extension '.atlas' will be added automatically)
            TexturePacker.Process( settings,
                                   $@"{IOData.InternalPath}\Assets\PackedImages\Objects",
                                   $@"{IOData.InternalPath}\Assets\PackedImages\output",
                                   "objects" );
            
//            TexturePacker.Process( settings, "packedimages/animations", "packedimages/output", "animations" );
//            TexturePacker.Process( settings, "packedimages/achievements", "packedimages/output", "achievements" );
//            TexturePacker.Process( settings, "packedimages/input", "packedimages/output", "buttons" );
//            TexturePacker.Process( settings, "packedimages/text", "packedimages/output", "text" );
        }
    }
    #endif
}

