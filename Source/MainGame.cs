//#define KEYBOARD
//#define OGL_TEST
//#define JSON_TEST
//#define PACK_IMAGES
//#define LOAD_ASSETS

#define FONTS

// ============================================================================

using System.Diagnostics;
using System.Runtime.Versioning;

using Extensions.Source.Tools.ImagePacker;

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Files;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.Text;
using LughSharp.Lugh.Graphics.Text.Freetype;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;

    private const int X = 40;
    private const int Y = 40;

    private          AssetManager?           _assetManager;
    private          Texture?                _image1;
    private          OrthographicGameCamera? _camera;
    private readonly Vector3                 _cameraPos = Vector3.Zero;
    private          SpriteBatch?            _spriteBatch;
    private          BitmapFont?             _font;

    #if OGL_TEST
    private readonly OpenGLTest _openGLTest = new();
    #endif

    #if JSON_TEST
    private readonly JsonTest _jsonTest = new();
    #endif

    #if PACK_IMAGES
    private const bool REBUILD_ATLAS = true;
    private const bool REMOVE_DUPLICATE_IMAGES = true;
    private const bool DRAW_DEBUG_LINES = false;
    #endif

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        _spriteBatch = new SpriteBatch();

        _camera = new OrthographicGameCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height )
        {
            CameraZoom       = 1f,
            PPM              = 32.0f,
            IsLerpingEnabled = false,
        };

        _camera.SetStretchViewport();
        _camera.SetZoomDefault( OrthographicGameCamera.DEFAULT_ZOOM );

        _assetManager = new AssetManager();
        _image1       = null;

        #if FONTS
        _font = CreateFont( "Assets/Fonts/ProFontWindows.ttf", 16 );
        #endif

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

//        IOUtils.DebugPaths();

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
            if ( _camera.IsInUse )
            {
                _camera.Viewport?.Apply();

                _spriteBatch.SetProjectionMatrix( _camera.Camera!.Combined );
                _spriteBatch.SetTransformMatrix( _camera.Camera.View );
                _spriteBatch.EnableBlending();
                _spriteBatch.Begin();

                _cameraPos.X = 0 + ( Gdx.GdxApi.Graphics.Width / 2f );
                _cameraPos.Y = 0 + ( Gdx.GdxApi.Graphics.Height / 2f );
                _cameraPos.Z = 0;

                _camera.SetPosition( _cameraPos );

                if ( _image1 != null )
                {
                    _spriteBatch.Draw( _image1, X, Y, _image1.Width, _image1.Height );
                }

                #if OGL_TEST
                _openGLTest.Render();
                #endif

                _spriteBatch.End();
            }
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        if ( _camera != null )
        {
            _camera.Camera!.ViewportWidth = width;
            _camera.Camera.ViewportHeight = height;
            _camera.Camera.Update();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _image1?.Dispose();
        _assetManager?.Dispose();

        GC.SuppressFinalize( this );
    }

    // ========================================================================
    // ========================================================================

    #if LOAD_ASSETS
    private void LoadAssets()
    {
        GdxRuntimeException.ThrowIfNull( _assetManager );

        Logger.Divider();
        Logger.Debug( "Loading assets...", true );
        Logger.Divider();

        _assetManager.Load( TEST_ASSET1, typeof( Texture ), new TextureLoader.TextureLoaderParameters() );
        _assetManager.FinishLoading();

        if ( _assetManager.Contains( TEST_ASSET1 ) )
        {
            _image1 = _assetManager.GetTexture( TEST_ASSET1 );
            Logger.Debug( "_image1 is SET" );
        }

        if ( _image1 != null )
        {
            Logger.Debug( "Asset loading failed" );
        }
    }
    #endif

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
                MaxWidth = 2048, // Maximum Width of final atlas image
                MaxHeight = 2048, // Maximum Height of final atlas image
                PowerOfTwo = true,
                Debug = DRAW_DEBUG_LINES,
                IsAlias = REMOVE_DUPLICATE_IMAGES,
            };

//            settings.WriteToJsonFile( "ExampleSettings.json" );
//            settings.ScaleResamplingTest4();
            
            // Build the Atlases from the specified parameters :-
            // - configuration settings
            // - source folder
            // - destination folder
            // - name of atlas, without extension (the extension '.atlas' will be added automatically)
            TexturePacker.Process( settings,
                                   @"\Assets\PackedImages\Objects",
                                   @"\Assets\PackedImages\output",
                                   "objects" );

//            TexturePacker.Process( settings, "packedimages/animations", "packedimages/output", "animations" );
//            TexturePacker.Process( settings, "packedimages/achievements", "packedimages/output", "achievements" );
//            TexturePacker.Process( settings, "packedimages/input", "packedimages/output", "buttons" );
//            TexturePacker.Process( settings, "packedimages/text", "packedimages/output", "text" );
        }
    }
    #endif

    public BitmapFont CreateFont( string fontFile, int size, Color color )
    {
        BitmapFont font;

        try
        {
            font = CreateFont( fontFile, size );
            font.SetColor( color );
        }
        catch ( Exception e )
        {
            Logger.Warning( e.Message );

            font = new BitmapFont();
        }

        return font;
    }

    public BitmapFont CreateFont( string fontFile, int size )
    {
        BitmapFont font;

        try
        {
            var generator = new FreeTypeFontGenerator( Gdx.GdxApi.Files.Internal( fontFile ) );
            var parameter = new FreeTypeFontGenerator.FreeTypeFontParameter()
            {
                Size = size,
            };

            font = generator.GenerateFont( parameter );
            font.SetColor( Color.White );
        }
        catch ( Exception e )
        {
            Logger.Warning( e.Message );

            font = new BitmapFont();
        }

        return font;
    }
}