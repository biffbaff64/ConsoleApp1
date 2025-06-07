using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Versioning;

using Extensions.Source.Tools.ImagePacker;

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Graphics.OpenGL.Enums;
using LughSharp.Lugh.Graphics.Text;
using LughSharp.Lugh.Input;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using Color = LughSharp.Lugh.Graphics.Color;
using PixelFormat = LughSharp.Lugh.Graphics.OpenGL.Enums.PixelFormat;
using PixelType = LughSharp.Lugh.Graphics.OpenGL.Enums.PixelType;

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
    private        SpriteBatch?            _spriteBatch;
    private        BitmapFont?             _font;
    private        InputMultiplexer?       _inputMultiplexer;
    private        Keyboard?               _keyboard;

    private uint _whitePixel;

    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        _assetManager = new AssetManager();
        _image1       = null;
        _font         = new BitmapFont();
        _spriteBatch  = new SpriteBatch();

        _orthoGameCam = new OrthographicGameCamera( Gdx.GdxApi.Graphics.Width, Gdx.GdxApi.Graphics.Height );
        _orthoGameCam.SetZoomDefault( CameraData.DEFAULT_ZOOM );
        _orthoGameCam.IsInUse = true;

        // ====================================================================
        // ====================================================================

        Logger.Debug( "Setting up Keyboard" );
        _keyboard         = new Keyboard();
        _inputMultiplexer = new InputMultiplexer();
        _inputMultiplexer.AddProcessor( _keyboard );
        Gdx.GdxApi.Input.InputProcessor = _inputMultiplexer;

        // ====================================================================

//        PackImages();

        // ====================================================================

//        LoadAssets();

        // ====================================================================

//        CreateWhitePixelTexture();

//        _image1 = new Texture( TEST_ASSET1 );

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

//        if ( ( _orthoGameCam != null ) && ( _spriteBatch != null ) )
//        {
//            if ( _orthoGameCam.IsInUse )
//            {
//                _orthoGameCam.Viewport?.Apply();
//
//                _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera!.Combined );
//                _spriteBatch.SetTransformMatrix( _orthoGameCam.Camera.View );
//                _spriteBatch.EnableBlending();
//                _spriteBatch.Begin();
//
//                _cameraPos.X = 0 + ( _orthoGameCam.Camera.ViewportWidth / 2f );
//                _cameraPos.Y = 0 + ( _orthoGameCam.Camera.ViewportHeight / 2f );
//                _cameraPos.Z = 0;
//
//                _orthoGameCam.SetPosition( _cameraPos );
//                _orthoGameCam.Update();
//
//                if ( _image1 != null )
//                {
//                    // Nothing is getting drawn here???
//                    _spriteBatch.Draw( _image1, 140, 210 );
//                }
//
//                DrawViewportBounds();
//                
//                _spriteBatch.End();
//            }
//        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        if ( _orthoGameCam != null )
        {
            _orthoGameCam.Camera!.ViewportWidth = width;
            _orthoGameCam.Camera.ViewportHeight = height;
            _orthoGameCam.Camera.Update();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _image1?.Dispose();
        _assetManager?.Dispose();
        _orthoGameCam?.Dispose();

        Gdx.GL.DeleteTextures( _whitePixel );

        GC.SuppressFinalize( this );
    }

    // ========================================================================
    // ========================================================================
    // ========================================================================

    private static void CheckViewportCoverage()
    {
        var viewport = new int[ 4 ];

        Gdx.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );

        var windowWidth  = Gdx.GdxApi.Graphics.Width;
        var windowHeight = Gdx.GdxApi.Graphics.Height;

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

    private void CreateWhitePixelTexture()
    {
        _whitePixel = Gdx.GL.GenTexture();

        Gdx.GL.BindTexture( TextureTarget.Texture2D, _whitePixel );

        // Create a single white pixel
        var pixel = new byte[] { 255, 255, 255, 255 }; // R,G,B,A = White, fully opaque

        // Upload the pixel data
        Gdx.GL.TexImage2D( ( int )TextureTarget.Texture2D,  // Target
                           0,                               // Level
                           ( int )PixelInternalFormat.Rgba, // Internal format
                           1,                               // Width (1 pixel)
                           1,                               // Height (1 pixel)
                           0,                               // Border
                           ( int )PixelFormat.Rgba,         // Format
                           ( int )PixelType.UnsignedByte,   // Type
                           pixel );                         // Data

        // Set texture parameters
        Gdx.GL.TexParameteri( ( int )TextureTarget.Texture2D,
                              ( int )TextureParameterName.TextureMinFilter,
                              ( int )TextureMinFilter.Nearest );

        Gdx.GL.TexParameteri( ( int )TextureTarget.Texture2D,
                              ( int )TextureParameterName.TextureMagFilter,
                              ( int )TextureMagFilter.Nearest );
    }

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thickness"></param>
    public void DrawViewportBounds( float thickness = 2f )
    {
        // Get the actual viewport dimensions
        var viewport = new int[ 4 ];
        Gdx.GL.GetIntegerv( ( int )GetPName.Viewport, ref viewport );

        var width  = viewport[ 2 ];
        var height = viewport[ 3 ];

        // Early exit if viewport has invalid dimensions
        if ( ( width <= 0 ) || ( height <= 0 ) )
        {
            Console.WriteLine( $"Warning: Invalid viewport dimensions: {width}x{height}" );

            return;
        }

        // Draw borders in different colors to easily identify edges
        // Top - Red
        _spriteBatch?.Draw( _whitePixel, new Rectangle( 0, 0, width, ( int )thickness ), Color.Red );

        // Bottom - Blue
        _spriteBatch?.Draw( _whitePixel, new Rectangle( 0, height - ( int )thickness, width, ( int )thickness ), Color.Blue );

        // Left - Green
        _spriteBatch?.Draw( _whitePixel, new Rectangle( 0, 0, ( int )thickness, height ), Color.Green );

        // Right - Yellow
        _spriteBatch?.Draw( _whitePixel, new Rectangle( width - ( int )thickness, 0, ( int )thickness, height ), Color.Yellow );
    }

    // ========================================================================

    private void DrawViewportBounds( SpriteBatch spriteBatch )
    {
        // Get the actual viewport dimensions
        var viewport = new int[ 4 ];

        Gdx.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );

        var width  = viewport[ 2 ];
        var height = viewport[ 3 ];

        spriteBatch.Begin();

        // Draw borders in different colors to easily identify edges
        var thickness = 2f; // Make it visible

        // Top - Red
        spriteBatch.Draw( _whitePixel, new Rectangle( 0, 0, width, ( int )thickness ), Color.White );

        // Bottom - Blue
        spriteBatch.Draw( _whitePixel, new Rectangle( 0, height - ( int )thickness, width, ( int )thickness ), Color.White );

        // Left - Green
        spriteBatch.Draw( _whitePixel, new Rectangle( 0, 0, ( int )thickness, height ), Color.White );

        // Right - Yellow
        spriteBatch.Draw( _whitePixel, new Rectangle( width - ( int )thickness, 0, ( int )thickness, height ), Color.White );

        spriteBatch.End();
    }

    // ========================================================================

    private void DebugViewportState()
    {
        var viewport = new int[ 4 ];
        Gdx.GL.GetIntegerv( ( int )GetPName.Viewport, ref viewport );

        Console.WriteLine( $"Viewport: X={viewport[ 0 ]}, Y={viewport[ 1 ]}, Width={viewport[ 2 ]}, Height={viewport[ 3 ]}" );

        // Check scissors test
        var scissorEnabled = Gdx.GL.IsEnabled( ( int )EnableCap.ScissorTest );
        Console.WriteLine( $"Scissor Test Enabled: {scissorEnabled}" );

        if ( scissorEnabled )
        {
            var scissors = new int[ 4 ];
            Gdx.GL.GetIntegerv( ( int )GetPName.ScissorBox, ref scissors );
            Console.WriteLine( $"Scissors: X={scissors[ 0 ]}, Y={scissors[ 1 ]}, Width={scissors[ 2 ]}, Height={scissors[ 3 ]}" );
        }
    }
}