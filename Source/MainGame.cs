using JetBrains.Annotations;

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Assets.Loaders;
using LughSharp.Lugh.Core;
using LughSharp.Lugh.Files;
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.Cameras;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Graphics.OpenGL.Enums;
using LughSharp.Lugh.Graphics.Utils;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Exceptions;

using Color = LughSharp.Lugh.Graphics.Color;

namespace ConsoleApp1.Source;

/// <summary>
/// TEST class, used for testing the framework.
/// </summary>
[PublicAPI]
public partial class MainGame : Game
{
    private const string TEST_ASSET1 = Assets.ROVER_WHEEL;
    private const int    TEST_WIDTH  = 100;
    private const int    TEST_HEIGHT = 100;

    // ========================================================================

    private readonly Vector3 _cameraPos = Vector3.Zero;

    private OrthographicGameCamera? _orthoGameCam;
    private SpriteBatch             _spriteBatch = null!;
    private AssetManager?           _assetManager;
    private Texture?                _image1;

    private Texture? _whitePixelTexture;

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override void Create()
    {
        Logger.Checkpoint();

        _assetManager = new AssetManager();
        _image1       = null;
        _spriteBatch  = new SpriteBatch();
        _spriteBatch.EnableBlending();
        _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );

        CreateCamera();

        // ====================================================================

        _image1 = new Texture( new FileInfo( $"{IOUtils.AssetsRoot}title_background.png" ) );

        if ( _image1 != null )
        {
            Logger.Debug( $"Texture loaded - Width: {_image1.Width}, Height: {_image1.Height}, " +
                          $"Format: {Gdx2DPixmap.GetFormatString( _image1.TextureData.PixelFormat )}" );
            ( _image1 as GLTexture )?.Bind(); // Force an initial bind

            if ( _image1 is GLTexture glTexture )
            {
                Logger.Debug( $"Texture binding test:" );
                glTexture.Bind( 0 ); // Bind to texture unit 0
                var error = Engine.GL.GetError();
                Logger.Debug( $"GL Error after bind: {error}" );

                unsafe
                {
                    var minFilter = new int[ 1 ];
                    var magFilter = new int[ 1 ];

                    // Check texture parameters
                    fixed ( int* minFilterPtr = minFilter )
                    {
                        Engine.GL.GetTexParameteriv( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MIN_FILTER, minFilterPtr );
                    }

                    fixed ( int* magFilterPtr = magFilter )
                    {
                        Engine.GL.GetTexParameteriv( IGL.GL_TEXTURE_2D, IGL.GL_TEXTURE_MAG_FILTER, magFilterPtr );
                    }

                    Logger.Debug( $"Texture filters - Min: {minFilter[ 0 ]}, Mag: {magFilter[ 0 ]}" );
                }
            }
        }

//        CreateImage1Texture();
//        CreateWhitePixelTexture();

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

        if ( _orthoGameCam is { IsInUse: true } )
        {
            _spriteBatch.Begin();
            _spriteBatch.EnableBlending();
            _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );

            _orthoGameCam.Viewport?.Apply();

            Logger.Debug( $"SpriteBatch state before Draw - IsDrawing: {_spriteBatch.IsDrawing}, BlendingEnabled: {_spriteBatch.BlendingEnabled}" );

            if ( _image1 is GLTexture glTexture )
            {
                Logger.Debug( $"GLTexture Handle: {glTexture.GLTextureHandle}, Target: {glTexture.GLTarget}" );
                glTexture.Bind(); // Force a bind before drawing
            }

            Logger.Debug( $"Shader active: {_spriteBatch.Shader != null}, " +
                          $"Matrix location: {_spriteBatch.Shader?.GetUniformLocation( "u_combinedMatrix" )}" );

            _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera.Combined );
            _orthoGameCam.Update();

            if ( _image1 != null )
            {
                if ( !_spriteBatch.BlendingEnabled )
                {
                    _spriteBatch.EnableBlending();
                    _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
                }

                Logger.Debug( $"Drawing texture at (40,40) with size {_image1.Width}x{_image1.Height}" );

                _spriteBatch.Draw( _image1,
                                   new GRect( 40, 40, 640, 480 ),                      // destination rectangle
                                   new GRect( 0, 0, _image1.Width, _image1.Height ) ); // source rectangle
                
                // Check GL state
                var viewport = new int[ 4 ];
                Engine.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );
                Logger.Debug( $"GL Viewport: ({viewport[ 0 ]}, {viewport[ 1 ]}, {viewport[ 2 ]}, {viewport[ 3 ]})" );
                Logger.Debug( $"SpriteBatch BlendingEnabled state: {_spriteBatch.BlendingEnabled}" );
                Logger.Debug( $"GL_BLEND Enabled: {Engine.GL.IsEnabled( IGL.GL_BLEND )}" );
                Logger.Debug( $"Depth Test: {Engine.GL.IsEnabled( IGL.GL_DEPTH_TEST )}" );

                _spriteBatch.DebugVertices();
            }

            _spriteBatch.End();
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
        Logger.Debug( $"Resizing to: {width}x{height}" );

        _orthoGameCam?.ResizeViewport( width, height );
    }

    private void CreateCamera()
    {
        _orthoGameCam = new OrthographicGameCamera( Engine.Api.Graphics.Width,
                                                    Engine.Api.Graphics.Height,
                                                    ppm: 1f );
        _orthoGameCam.Camera.Near = 1.0f;
        _orthoGameCam.Camera.Far  = 100.0f;
        _orthoGameCam.IsInUse     = true;
        _orthoGameCam.SetZoomDefault( CameraData.DEFAULT_ZOOM );

        // Set initial camera position
        _cameraPos.X = 0f;
        _cameraPos.Y = 0f;
        _cameraPos.Z = 0f;
        _orthoGameCam.SetPosition( _cameraPos );
        _orthoGameCam.Update();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _spriteBatch.Dispose();
            _image1?.Dispose();
            _whitePixelTexture?.Dispose();
            _assetManager?.Dispose();
            _orthoGameCam?.Dispose();
        }
    }
}

// ============================================================================
// ============================================================================