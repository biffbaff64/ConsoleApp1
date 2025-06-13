using ConsoleApp1.Source.Tests;

using DotGLFW;

using Extensions.Source.Tools.ImagePacker;

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Graphics.OpenGL.Enums;
using LughSharp.Lugh.Graphics.Utils;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using Color = LughSharp.Lugh.Graphics.Color;
using PixelType = LughSharp.Lugh.Graphics.Images.PixelType;

namespace ConsoleApp1.Source;

/// <summary>
/// TEST class, used for testing the framework.
/// </summary>
public class MainGame : ApplicationAdapter
{
    private const string TEST_ASSET1             = Assets.ROVER_WHEEL;
    private const bool   REBUILD_ATLAS           = true;
    private const bool   REMOVE_DUPLICATE_IMAGES = true;
    private const bool   DRAW_DEBUG_LINES        = false;

    // ========================================================================

    private readonly Vector3 _cameraPos = Vector3.Zero;

    private static OrthographicGameCamera? _orthoGameCam;
    private        AssetManager?           _assetManager;
    private        Texture?                _image1;

    private SpriteBatch? _spriteBatch;
    private SpriteBatch? _spriteBatch2;

//    private        BitmapFont?             _font;
//    private        InputMultiplexer?       _inputMultiplexer;
//    private        Keyboard?               _keyboard;

    private Texture? _whitePixelTexture;

    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        _assetManager = new AssetManager();
        _image1       = null;
//        _spriteBatch  = new SpriteBatch();
//
//        _orthoGameCam = new OrthographicGameCamera( Engine.Api.Graphics.Width,
//                                                    Engine.Api.Graphics.Height,
//                                                    ppm: 1f );
//        _orthoGameCam.SetZoomDefault( CameraData.DEFAULT_ZOOM );
//        _orthoGameCam.IsInUse = true;

        // ====================================================================
        // ====================================================================

//        Logger.Debug( "Setting up Keyboard" );
//        _keyboard         = new Keyboard();
//        _inputMultiplexer = new InputMultiplexer();
//        _inputMultiplexer.AddProcessor( _keyboard );
//        Engine.Api.Input.InputProcessor = _inputMultiplexer;

        // ====================================================================

//        PackImages();

        // ====================================================================

//        LoadAssets();

        Logger.Debug( "Creating SpriteBatch tests." );
        
        var test = new SpriteBatchTests();
        test.Setup();

        Logger.Debug( "SpriteBatch tests starting" );

        try
        {
            test.Begin_WhenCalled_SetsCorrectDefaultState();
            test.Draw_WhenCalledBetweenBeginAndEnd_SuccessfullyDrawsTexture();
//            test.Draw_WhenCalledOutsideBeginEnd_ThrowsException();
            test.SetProjectionMatrix_UpdatesMatrixCorrectly();
            test.Blending_CanBeToggledCorrectly();
            test.Color_CanBeSetAndRetrieved();
        }
        catch ( Exception ex )
        {
            Logger.Debug( $"Exception thrown: {ex.Message}" );
        }
        finally
        {
            test.Teardown();
        }
        
        Logger.Debug("SpriteBatch tests completed");
        
        Logger.Debug( "Done" );
    }

    // ========================================================================

    /// <inheritdoc />
    public override void Update()
    {
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue );

//        if ( ( _orthoGameCam != null ) && ( _spriteBatch != null ) )
//        {
//            if ( _orthoGameCam.IsInUse )
//            {
//                _orthoGameCam.Update();
//                _orthoGameCam.Viewport?.Apply();
//
//                _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera.Combined );
//                _spriteBatch.SetTransformMatrix( _orthoGameCam.Camera.ViewMatrix );
//                _spriteBatch.EnableBlending();
//
//                // ------------------------------
//                
//                Engine.GL.BlendFunc( ( int )BlendingFactor.SrcAlpha, ( int )BlendingFactor.OneMinusSrcAlpha );
//                Engine.GL.Disable((int)EnableCap.DepthTest);
//                Engine.GL.DepthMask(false);
//
//                // ------------------------------
//                
//                _spriteBatch.Begin();
//                _orthoGameCam.Position.Set( 0, 0, 0 );
//
//                // ------------------------------
//                
//                if ( _image1 != null )
//                {
//                    _spriteBatch.Draw( _image1, 100, 100, 68, 68 );
//                }
//
//                Engine.GL.Enable((int)EnableCap.DepthTest);
//                Engine.GL.DepthMask(true);
//
//                // ------------------------------
//                
//                _spriteBatch.End();
//
//                // ------------------------------
//
//                // Check for any GL errors at the end of the frame
//                var error = Engine.GL.GetError();
//
//                if ( error != ( int )ErrorCode.NoError )
//                {
//                    Logger.Debug( $"GL Error at end of frame: {error}" );
//                }
//            }
//        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        if ( _orthoGameCam != null )
        {
            Logger.Debug( $"Current viewport: {_orthoGameCam.Camera.ViewportWidth}"
                          + $"x{_orthoGameCam.Camera.ViewportHeight}" );
            Logger.Debug( $"Resizing to {width}x{height}, CentreCamera = false" );

            _orthoGameCam.ResizeViewport( width, height );
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _image1?.Dispose();
        _assetManager?.Dispose();
        _orthoGameCam?.Dispose();

        GC.SuppressFinalize( this );
    }

    // ========================================================================
    // ========================================================================
    // ========================================================================

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

            #if DEBUG
            Logger.Debug( $"Loaded image type: {_image1.GetType()}" );

            var data = _image1.GetImageData();

            if ( data != null )
            {
                for ( var i = 0; i < 20; i++ )
                {
                    for ( var j = 0; j < 20; j++ )
                    {
                        Logger.Data( $"[{data[ ( i * 20 ) + j ]:X}]", false );
                    }

                    Logger.NewLine();
                }
            }
            #endif
        }
    }

    // ========================================================================

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

//            settings.WriteToJsonFile( "ExampleSettings.json" );

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

    // ========================================================================
    // ========================================================================
    // ========================================================================

    private static void CheckViewportCoverage()
    {
        var viewport = new int[ 4 ];

        Engine.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );

        var windowWidth  = Engine.Api.Graphics.Width;
        var windowHeight = Engine.Api.Graphics.Height;

        var isFullyCovered = ( viewport[ 0 ] == 0 )                // Left edge at 0
                             && ( viewport[ 1 ] == 0 )             // Bottom edge at 0
                             && ( viewport[ 2 ] == windowWidth )   // Width matches
                             && ( viewport[ 3 ] == windowHeight ); // Height matches

        if ( !isFullyCovered )
        {
            Logger.Debug( "WARNING: Viewport doesn't cover entire window!" );
            Logger.Debug( $"Window: {windowWidth}x{windowHeight}" );
            Logger.Debug( $"Viewport: {viewport[ 2 ]}x{viewport[ 3 ]} at ({viewport[ 0 ]},{viewport[ 1 ]})" );

            if ( ( viewport[ 0 ] != 0 ) || ( viewport[ 1 ] != 0 ) )
            {
                Logger.Debug( "Viewport is offset from window origin!" );
            }

            if ( ( viewport[ 2 ] != windowWidth ) || ( viewport[ 3 ] != windowHeight ) )
            {
                Logger.Debug( "Viewport size doesn't match window size!" );
            }
        }
    }

    // ========================================================================

    private Texture CreateWhitePixelTexture()
    {
        if ( _whitePixelTexture != null )
        {
            return _whitePixelTexture;
        }

        Logger.Debug( "Creating white pixel texture" );

        var pixmap = new Pixmap( 1, 1, PixelType.Format.RGBA8888 );
        pixmap.SetColor( Color.White );
        pixmap.FillWithCurrentColor();

        var textureData = new PixmapTextureData( pixmap, PixelType.Format.RGBA8888, false, false );

        _whitePixelTexture = new Texture( textureData );

        Logger.Debug( $"Created white pixel texture with handle: {_whitePixelTexture.GLTextureHandle}" );

        return _whitePixelTexture;
    }

    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thickness"></param>
    public void DrawViewportBounds( float thickness = 2f )
    {
        if ( ( _spriteBatch == null ) || ( _whitePixelTexture == null ) )
        {
            Logger.Debug( "SpriteBatch or white pixel texture not initialized" );

            return;
        }

        // Get and verify viewport dimensions
        var viewport = new int[ 4 ];
        Engine.GL.GetIntegerv( ( int )GetPName.Viewport, ref viewport );
        Logger.Debug( $"Viewport dimensions: {viewport[ 2 ]}x{viewport[ 3 ]}" );

        var width  = viewport[ 2 ];
        var height = viewport[ 3 ];

        if ( ( width <= 0 ) || ( height <= 0 ) )
        {
            Logger.Debug( $"Invalid viewport dimensions: {width}x{height}" );

            return;
        }

        try
        {
            // Make sure SpriteBatch is in the correct state
            if ( !_spriteBatch.IsDrawing )
            {
                Logger.Debug( "Starting SpriteBatch" );
                _spriteBatch.Begin();
            }

            // Draw the bounds
            if ( _whitePixelTexture != null )
            {
//                _spriteBatch.Draw( _image1, 0, 0 );
                _spriteBatch.Draw( _whitePixelTexture, 0, 0 );
            }

            // Check for GL errors after drawing
            var error = Engine.GL.GetError();

            if ( error != ( int )ErrorCode.NoError )
            {
                Logger.Debug( $"GL Error during drawing: {error}" );
            }
        }
        catch ( Exception ex )
        {
            Logger.Debug( $"Error during drawing: {ex.Message}" );
        }
    }

    // ========================================================================

    private void DrawViewportBounds( SpriteBatch spriteBatch )
    {
        // Get the actual viewport dimensions
        var viewport = new int[ 4 ];

        Engine.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );

        var width  = viewport[ 2 ];
        var height = viewport[ 3 ];

        // Draw borders in different colors to easily identify edges
        var thickness = 2f; // Make it visible

        if ( _whitePixelTexture != null )
        {
//            spriteBatch.Draw( _whitePixelTexture, new Rectangle( 0, 0, width, ( int )thickness ) );
//            spriteBatch.Draw( _whitePixelTexture, new Rectangle( 0, height - ( int )thickness, width, ( int )thickness ) );
//            spriteBatch.Draw( _whitePixelTexture, new Rectangle( 0, 0, ( int )thickness, height ) );
//            spriteBatch.Draw( _whitePixelTexture, new Rectangle( width - ( int )thickness, 0, ( int )thickness, height ) );
        }
    }

    // ========================================================================

    private void DebugViewportState()
    {
        var viewport = new int[ 4 ];
        Engine.GL.GetIntegerv( ( int )GetPName.Viewport, ref viewport );

        Logger.Debug( $"Viewport: X={viewport[ 0 ]}, Y={viewport[ 1 ]}, Width={viewport[ 2 ]}, Height={viewport[ 3 ]}" );

        // Check scissors test
        var scissorEnabled = Engine.GL.IsEnabled( ( int )EnableCap.ScissorTest );
        Logger.Debug( $"Scissor Test Enabled: {scissorEnabled}" );

        if ( scissorEnabled )
        {
            var scissors = new int[ 4 ];
            Engine.GL.GetIntegerv( ( int )GetPName.ScissorBox, ref scissors );
            Logger.Debug( $"Scissors: X={scissors[ 0 ]}, Y={scissors[ 1 ]}, Width={scissors[ 2 ]}, Height={scissors[ 3 ]}" );
        }
    }
}

