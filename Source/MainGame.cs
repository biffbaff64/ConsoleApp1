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
                          $"Format: {PixelFormatUtils.GetFormatString( _image1.TextureData.PixelFormat )}" );

            _image1.Upload();
            _image1.Bind( 0 ); // Set active texture and bind to texture unit 0

            var width  = new int[ 1 ];
            var height = new int[ 1 ];
            Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_WIDTH, ref width );
            Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_HEIGHT, ref height );
            Logger.Debug( $"Initial texture dimensions in GPU: {width[ 0 ]}x{height[ 0 ]}" );
            
            Engine.GL.TexParameteri( ( int )TextureTarget.Texture2D,
                                     ( int )TextureParameter.MinFilter,
                                     ( int )TextureFilterMode.Nearest );
            Engine.GL.TexParameteri( ( int )TextureTarget.Texture2D,
                                     ( int )TextureParameter.MagFilter,
                                     ( int )TextureFilterMode.Nearest );
            
            Logger.Debug( "TextureMinFilter set to GL_NEAREST" );
            Logger.Debug( "TextureMagFilter set to GL_NEAREST" );
        }

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
        ScreenUtils.Clear( Color.Blue, clearDepth: true );

        if ( _orthoGameCam is { IsInUse: true } )
        {
            _spriteBatch.Shader?.Bind();
            _spriteBatch.SetupVertexAttributes( _spriteBatch.Shader );

            _spriteBatch.Begin();
            _spriteBatch.EnableBlending();
            _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );

            _orthoGameCam.Viewport?.Apply();
            _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera.Combined );
            _orthoGameCam.Update();

            if ( _image1 != null )
            {
                _spriteBatch.Draw( _image1, 0, 0 );
            }

            _spriteBatch.End();
        }
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
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
        Logger.Checkpoint();
        
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    protected override void Dispose( bool disposing )
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