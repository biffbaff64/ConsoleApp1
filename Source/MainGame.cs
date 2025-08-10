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
            Logger.Debug( $"TextureData prepared: {_image1.TextureData.IsPrepared}" );
            Logger.Debug( $"TextureData consumable: {_image1.TextureData.IsManaged}" );
            Logger.Debug( $"TextureData type: {_image1.TextureData.GetType().Name}" );
        }

        if ( _image1 != null )
        {
            Logger.Debug( $"Texture loaded - Width: {_image1.Width}, Height: {_image1.Height}, " +
                          $"Format: {PixelFormatUtils.GetFormatString( _image1.TextureData.PixelFormat )}" );

            // Force texture data upload if needed
            if ( !_image1.TextureData.IsPrepared )
            {
                _image1.TextureData.Prepare();
            }

            ( _image1 as GLTexture )?.Bind(); // Force an initial bind

            var width  = new int[ 1 ];
            var height = new int[ 1 ];
            Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_WIDTH, ref width );
            Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_HEIGHT, ref height );
            Logger.Debug( $"Initial texture dimensions in GPU: {width[ 0 ]}x{height[ 0 ]}" );

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
//        ScreenUtils.Clear( Color.Blue, clearDepth: true );

        if ( _orthoGameCam is { IsInUse: true } )
        {
            _spriteBatch.Shader?.Bind();
            _spriteBatch.SetupVertexAttributes( _spriteBatch.Shader );

            _spriteBatch.Begin();
            _spriteBatch.EnableBlending();
            _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );

            _orthoGameCam.Viewport?.Apply();

            #if RENDER_DEBUG
            Logger.Debug( $"SpriteBatch state before Draw - IsDrawing: {_spriteBatch.IsDrawing}, BlendingEnabled: {_spriteBatch.BlendingEnabled}" );
            #endif
            
            if ( _image1 is GLTexture glTexture )
            {
                #if RENDER_DEBUG
                Logger.Debug( $"GLTexture Handle: {glTexture.GLTextureHandle}, Target: {glTexture.GLTarget}" );
                #endif
                
                // Make sure texture data is uploaded
                if ( !_image1.TextureData.IsPrepared )
                {
                    _image1.TextureData.Prepare();
                }

                Engine.GL.ActiveTexture( TextureUnit.Texture0 ); // Select texture unit 0

                glTexture.Bind(); // Force a bind before drawing

                // Verify texture dimensions after binding
                #if RENDER_DEBUG
                var width  = new int[ 1 ];
                var height = new int[ 1 ];
                Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_WIDTH, ref width );
                Engine.GL.GetTexLevelParameteriv( IGL.GL_TEXTURE_2D, 0, IGL.GL_TEXTURE_HEIGHT, ref height );
                Logger.Debug( $"Render-time texture dimensions in GPU: {width[ 0 ]}x{height[ 0 ]}" );
                #endif
            }

            #if RENDER_DEBUG
            Logger.Debug( $"Shader active: {_spriteBatch.Shader != null}, " +
                          $"Matrix location: {_spriteBatch.Shader?.GetUniformLocation( "u_combinedMatrix" )}" );
            #endif
            
//            _spriteBatch.SetProjectionMatrix( _orthoGameCam.Camera.Combined );
            _spriteBatch.SetProjectionMatrix( Matrix4.Identity );
            _orthoGameCam.Update();

            if ( _image1 != null )
            {
                if ( !_spriteBatch.BlendingEnabled )
                {
                    _spriteBatch.EnableBlending();
                    _spriteBatch.SetBlendFunction( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
                }

                #if RENDER_DEBUG
                Logger.Debug( $"Drawing texture at (40,40) with size {_image1.Width}x{_image1.Height}" );

                var vport = new int[ 4 ];
                Engine.GL.GetIntegerv( ( int )GLParameter.Viewport, ref vport );
                Logger.Debug( $"Viewport: x={vport[ 0 ]}, y={vport[ 1 ]}, width={vport[ 2 ]}, height={vport[ 3 ]}" );

                var blend = new bool[ 1 ];
                Engine.GL.GetBooleanv( ( int )GLParameter.Blend, ref blend );
                Logger.Debug( $"Blending enabled: {blend[ 0 ]}" );

                Engine.GL.GetBooleanv( ( int )GLParameter.DepthTest, out var dt );
                Logger.Debug( $"Depth test enabled: {dt}" );
                #endif
                
                // Get and set the texture uniform location
                Engine.GL.ActiveTexture( TextureUnit.Texture0 );                           // Select texture unit 0
                Engine.GL.BindTexture( TextureTarget.Texture2D, _image1.GLTextureHandle ); // Bind actual texture to unit 0
                
                #if RENDER_DEBUG
                Logger.Debug( $"ShaderProgramHandle: {_spriteBatch.Shader?.ShaderProgramHandle}" );
                _spriteBatch.Shader?.SetUniformi( "u_texture", 0 ); // Tell shader to use texture unit 0

                var blendSrc = new int[ 1 ];
                var blendDst = new int[ 1 ];
                Engine.GL.GetIntegerv( IGL.GL_BLEND_SRC, ref blendSrc );
                Engine.GL.GetIntegerv( IGL.GL_BLEND_DST, ref blendDst );
                Logger.Checkpoint();
                Logger.Debug( $"Blend functions - Src: {blendSrc[ 0 ]}, Dst: {blendDst[ 0 ]}" );
                #endif
                
                Engine.GL.Enable( IGL.GL_DEPTH_TEST );
                Engine.GL.DepthFunc( IGL.GL_LEQUAL );

                #if RENDER_DEBUG
                Engine.GL.GetIntegerv( ( int )GLParameter.DrawFramebufferBinding, out var drawFbo );
                Logger.Debug( $"Draw FBO = {drawFbo}" ); // should be 0 for default
                #endif
                
                _spriteBatch.Draw( _image1,
                                   new GRect( 40, 40, 640, 480 ),                      // destination rectangle
                                   new GRect( 0, 0, _image1.Width, _image1.Height ) ); // source rectangle

                Engine.GL.Disable( IGL.GL_DEPTH_TEST );

                #if RENDER_DEBUG
                var activeTexUnit = new int[ 1 ];
                var boundTexture  = new int[ 1 ];
                Engine.GL.GetIntegerv( IGL.GL_ACTIVE_TEXTURE, ref activeTexUnit );
                Engine.GL.GetIntegerv( IGL.GL_TEXTURE_BINDING_2D, ref boundTexture );
                Logger.Debug( $"Active texture unit: {activeTexUnit[ 0 ] - IGL.GL_TEXTURE0}, Bound texture: {boundTexture[ 0 ]}" );

                for ( uint i = 0; i < 3; i++ )
                {
                    var enabled = new int[ 1 ];
                    Engine.GL.GetVertexAttribiv( i, IGL.GL_VERTEX_ATTRIB_ARRAY_ENABLED, ref enabled );
                    Logger.Debug( $"Vertex attribute {i} enabled: {enabled[ 0 ] != 0}" );
                }

                // Check GL state
                var viewport = new int[ 4 ];
                Engine.GL.GetIntegerv( IGL.GL_VIEWPORT, ref viewport );
                Logger.Debug( $"GL Viewport: ({viewport[ 0 ]}, {viewport[ 1 ]}, {viewport[ 2 ]}, {viewport[ 3 ]})" );
                Logger.Debug( $"SpriteBatch BlendingEnabled state: {_spriteBatch.BlendingEnabled}" );
                Logger.Debug( $"GL_BLEND Enabled: {Engine.GL.IsEnabled( IGL.GL_BLEND )}" );
                Logger.Debug( $"Depth Test: {Engine.GL.IsEnabled( IGL.GL_DEPTH_TEST )}" );

                _spriteBatch.DebugVertices();
                #endif
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