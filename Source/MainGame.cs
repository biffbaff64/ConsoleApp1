using DotGLFW;

using JetBrains.Annotations;

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Graphics.OpenGL.Enums;
using LughSharp.Lugh.Graphics.Text;
using LughSharp.Lugh.Graphics.Utils;
using LughSharp.Lugh.Input;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using Color = LughSharp.Lugh.Graphics.Color;

namespace ConsoleApp1.Source;

/// <summary>
/// TEST class, used for testing the framework.
/// </summary>
[PublicAPI]
public class MainGame : Game
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;
    private const int    TEST_WIDTH  = 100;
    private const int    TEST_HEIGHT = 100;

    // ========================================================================

    private readonly Vector3                 _cameraPos = Vector3.Zero;
    private static   OrthographicGameCamera? _orthoGameCam;
    private          AssetManager?           _assetManager;
    private          Texture?                _image1;
    private          SpriteBatch?            _spriteBatch;
    private          Texture?                _whitePixelTexture;
//    private          Texture?                _testTexture;
//    private          BitmapFont?             _font;
//    private          InputMultiplexer?       _inputMultiplexer;
//    private          Keyboard?               _keyboard;

    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        Logger.Checkpoint();
        Logger.Debug($"Window dimensions: {Engine.Api.Graphics.Width}x{Engine.Api.Graphics.Height}");
        
        _assetManager = new AssetManager();
        _image1       = null;
        _spriteBatch  = new SpriteBatch();

        if ( _spriteBatch == null )
        {
            Logger.Warning( "Failed to create SpriteBatch" );

            return;
        }

        _orthoGameCam = new OrthographicGameCamera( Engine.Api.Graphics.Width,
                                                    Engine.Api.Graphics.Height,
                                                    ppm: 1f );
        _orthoGameCam.SetZoomDefault( CameraData.DEFAULT_ZOOM );
        _orthoGameCam.IsInUse = true;

        // Set initial camera position
        _cameraPos.X = Engine.Api.Graphics.Width / 2f;
        _cameraPos.Y = Engine.Api.Graphics.Height / 2f;
        _cameraPos.Z = 0f;
        _orthoGameCam.SetPosition(_cameraPos);

        // ====================================================================

        CreateImage1Texture();
        CreateWhitePixelTexture();
        
        // ====================================================================

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
        // Clear and set viewport
        ScreenUtils.Clear( Color.Blue, clearDepth: false );

        if ( ( _spriteBatch != null ) && _orthoGameCam is { IsInUse: true } )
        {
            _spriteBatch.EnableBlending();
            _orthoGameCam.Viewport?.Apply();
            _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera.Combined );
            _spriteBatch.Begin( depthMaskEnabled: false );

            if ( _image1 != null )
            {
                _spriteBatch.Draw( _image1, 40, 40 );
            }

            _orthoGameCam.Update();
            _spriteBatch.End();
            
            CheckViewportCoverage();
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        Logger.Debug($"Resizing to: {width}x{height}");
    
        if (_orthoGameCam != null)
        {
            _orthoGameCam.ResizeViewport(width, height);
        
            // Update camera position to new center
            _cameraPos.X = width / 2f;
            _cameraPos.Y = height / 2f;
            _orthoGameCam.SetPosition(_cameraPos);
        
            // Force an update after resize
            _orthoGameCam.Update();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _spriteBatch?.Dispose();
        _image1?.Dispose();
        _whitePixelTexture?.Dispose();
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

    private void CreateImage1Texture()
    {
        var pixmap = new Pixmap( TEST_WIDTH, TEST_HEIGHT, Gdx2DPixmap.Gdx2DPixmapFormat.RGBA8888 );
        pixmap.SetColor( Color.Magenta );
        pixmap.FillWithCurrentColor();

        _image1      = new Texture( new PixmapTextureData( pixmap, Gdx2DPixmap.Gdx2DPixmapFormat.RGBA8888, false, false ) );
        _image1.Name = "TestImage";
        
        // Set texture parameters
        Engine.GL.BindTexture( IGL.GL_TEXTURE_2D, _image1.TextureID );
        Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MIN_FILTER, IGL.GL_NEAREST );
        Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MAG_FILTER, IGL.GL_NEAREST );
        Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_WRAP_S, IGL.GL_CLAMP_TO_EDGE );
        Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_WRAP_T, IGL.GL_CLAMP_TO_EDGE );

        pixmap.Dispose();

        // Validate texture creation
        if ( !Engine.GL.IsGLTexture( _image1.TextureID ) )
        {
            Logger.Debug( "Failed to create texture" );

            return;
        }

        _image1.Debug();
    }

    // ========================================================================
    
    private void CreateWhitePixelTexture()
    {
        Logger.Checkpoint();
        
        if ( _whitePixelTexture != null )
        {
            return;
        }

        var pixmap = new Pixmap( 100, 100, Gdx2DPixmap.Gdx2DPixmapFormat.RGBA8888 );
        pixmap.SetColor( Color.White );
        pixmap.FillWithCurrentColor();

        var textureData = new PixmapTextureData( pixmap, Gdx2DPixmap.Gdx2DPixmapFormat.RGBA8888, false, false );

        _whitePixelTexture = new Texture( textureData );
        _whitePixelTexture.Name = "WhitePixel";

        if ( _whitePixelTexture != null )
        {
            // Set texture parameters
            Engine.GL.BindTexture( IGL.GL_TEXTURE_2D, _whitePixelTexture.TextureID );
            Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MIN_FILTER, IGL.GL_NEAREST );
            Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MAG_FILTER, IGL.GL_NEAREST );
            Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_WRAP_S, IGL.GL_CLAMP_TO_EDGE );
            Engine.GL.TexParameteri( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_WRAP_T, IGL.GL_CLAMP_TO_EDGE );
            
            // Validate texture creation
            if ( !Engine.GL.IsGLTexture( _whitePixelTexture.TextureID ) )
            {
                throw new GdxRuntimeException( "Failed to create texture" );
            }
            
            _whitePixelTexture.Debug();
        }
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
//        Logger.Debug( $"Viewport dimensions: {viewport[ 2 ]}x{viewport[ 3 ]}" );

        var width  = viewport[ 2 ];
        var height = viewport[ 3 ];

        if ( ( width <= 0 ) || ( height <= 0 ) )
        {
            Logger.Debug( $"Invalid viewport dimensions: {width}x{height}" );

            return;
        }

        try
        {
            if ( _whitePixelTexture != null )
            {
                _spriteBatch.Draw( _whitePixelTexture, width / 2f, height / 2f );
                GLUtils.CheckGLError( "MainGame::DrawViewportBounds" );
            }
        }
        catch ( Exception ex )
        {
            Logger.Debug( $"Error during drawing: {ex.Message}" );
        }
    }

    // ========================================================================

//    private void DebugViewportState()
//    {
//        var viewport = new int[ 4 ];
//        Engine.GL.GetIntegerv( ( int )GetPName.Viewport, ref viewport );
//
//        Logger.Debug( $"Viewport: X={viewport[ 0 ]}, Y={viewport[ 1 ]}, Width={viewport[ 2 ]}, Height={viewport[ 3 ]}" );
//
//        // Check scissors test
//        var scissorEnabled = Engine.GL.IsEnabled( ( int )EnableCap.ScissorTest );
//        Logger.Debug( $"Scissor Test Enabled: {scissorEnabled}" );
//
//        if ( scissorEnabled )
//        {
//            var scissors = new int[ 4 ];
//
//            Engine.GL.GetIntegerv( ( int )GetPName.ScissorBox, ref scissors );
//
//            Logger.Debug( $"Scissors: X={scissors[ 0 ]}, Y={scissors[ 1 ]}, " +
//                          $"Width={scissors[ 2 ]}, Height={scissors[ 3 ]}" );
//        }
//    }
}