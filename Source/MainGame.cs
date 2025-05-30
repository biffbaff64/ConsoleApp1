﻿//#define KEYBOARD
//#define CAMERA
//#define PACK_IMAGES
//#define LOAD_ASSETS

// ============================================================================

using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;

using Extensions.Source.Tools.ImagePacker;

using LughSharp.Tests.Source;
using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Files;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.Text;
using LughSharp.Lugh.Graphics.Utils;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

namespace ConsoleApp1.Source;

public class MainGame : ApplicationAdapter
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;

    // ========================================================================

    #if CAMERA
    private readonly Vector3 _cameraPos = Vector3.Zero;
//    private OrthographicGameCamera? _camera;
    #endif
    
    private AssetManager?           _assetManager;
    private Texture?                _image1;
    private SpriteBatch?            _spriteBatch;
    private BitmapFont?             _font;

    // ========================================================================

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
        _assetManager = new AssetManager();
        _image1       = null;
        _font         = new BitmapFont();
        _spriteBatch  = new SpriteBatch();

        #if CAMERA
        _camera = new OrthographicGameCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height )
        {
            CameraZoom       = 1f,
            PPM              = 32.0f,
            IsLerpingEnabled = false,
        };

        _camera.SetStretchViewport();
        _camera.SetZoomDefault( OrthographicGameCamera.DEFAULT_ZOOM );
        _camera.IsInUse = true;
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

        _image1 = new Texture( TEST_ASSET1 );
        _image1.Debug();

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

        if ( /*( _camera != null ) &&*/ ( _spriteBatch != null ) )
        {
//            if ( _camera.IsInUse )
//            {
//                _camera.Viewport?.Apply();

//                _spriteBatch.SetProjectionMatrix( _camera.Camera!.Combined );
//                _spriteBatch.SetTransformMatrix( _camera.Camera.View );
//                _spriteBatch.EnableBlending();
                _spriteBatch.Begin();

//                _cameraPos.X = 0 + ( _camera.Camera.ViewportWidth / 2f );
//                _cameraPos.Y = 0 + ( _camera.Camera.ViewportHeight / 2f );
//                _cameraPos.Z = 0;

//                _camera.SetPosition( _cameraPos );

                if ( _image1 != null )
                {
                    _spriteBatch.Draw( _image1, 140, 210 );
                }

//                _font?.Draw( _spriteBatch, "Hello World!", 10, 10 );

                _spriteBatch.End();
//            }
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
//        if ( _camera != null )
//        {
//            _camera.Camera!.ViewportWidth = width;
//            _camera.Camera.ViewportHeight = height;
//            _camera.Camera.Update();
//        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _image1?.Dispose();
        _assetManager?.Dispose();
//        _camera?.Dispose();

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
            _image1 = _assetManager.GetAs< Texture >( TEST_ASSET1 );
        }

        if ( _image1 == null )
        {
            Logger.Debug( "Asset loading failed" );
        }
        else
        {
            Logger.Debug( "Asset loaded" );

//            var data = _image1.GetImageData();
//
//            if ( data != null )
//            {
//                for ( var i = 0; i < 20; i++ )
//                {
//                    for ( var j = 0; j < 20; j++ )
//                    {
//                        Logger.Data( $"[{data[ ( i * 20 ) + j ]:X}]", false );
//                    }
//                    
//                    Logger.NewLine();
//                }
//            }
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
}