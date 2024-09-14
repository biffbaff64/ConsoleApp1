using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Utils;

namespace ConsoleApp1.Source;

// ReSharper disable once MemberCanBeInternal
public class MainGame : ApplicationAdapter
{
    public InputMultiplexer?   InputMultiplexer { get; set; }
    public Keyboard?           Keyboard         { get; set; }
    public OrthographicCamera? Camera           { get; set; }
    public SpriteBatch?        SpriteBatch      { get; set; }
    public Texture?            Background       { get; set; }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override void Create()
    {
        Logger.CheckPoint();

        SpriteBatch = new SpriteBatch();
        Camera      = new OrthographicCamera();
        Camera.SetToOrtho( false, Gdx.Graphics.Width, Gdx.Graphics.Height );
        Camera.Zoom = 0f;

        // --------------------------------------------------------------------
        // Working
//        var pm = new Pixmap( 100, 100, Pixmap.ColorFormat.RGBA8888 );
//        _background = new Texture( 100, 100, Pixmap.ColorFormat.RGBA8888 );

        // --------------------------------------------------------------------
        // Not Working
        var pm = new Pixmap( new FileInfo( Gdx.Files.Internal( "red7logo_small.png" ).FileName ) );
        Background = new Texture( pm );

//        _background = new Texture( "red7logo_small.png" );

        Logger.Debug( $"pm.width: {pm.Width}, pm.height: {pm.Height}" );
        Logger.Debug( $"pm.Format: {pm.Format}" );
        Logger.Debug( $"pm.Pixels.BackingArray: {pm.Pixels?.BackingArray().Length}" );

        if ( Background != null )
        {
            Logger.Debug( $"_background: {Background.Width} x {Background.Height}" );
            Logger.Debug( $"_background Format: {Background.TextureData?.Format}" );
        }

        Logger.CheckPoint( true );
        Logger.Debug( "Setting up Keyboard" );

        Keyboard         = new Keyboard();
        InputMultiplexer = new InputMultiplexer();
        InputMultiplexer.AddProcessor( Keyboard );
        Gdx.Input.InputProcessor = InputMultiplexer;

        Logger.CheckPoint();
    }

//    /// <inheritdoc />
//    public override void Update()
//    {
//        base.Update();
//    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( Color.Blue );

        if ( ( Camera != null ) && ( SpriteBatch != null ) )
        {
            Camera.Update();

            SpriteBatch.SetProjectionMatrix( Camera.Combined );
            SpriteBatch.Begin();

            if ( Background != null )
            {
                SpriteBatch.Draw( Background, 0, 0 );
            }

            SpriteBatch.End();
            SpriteBatch.EnableBlending();
        }
    }

    /// <inheritdoc />
    public override void Pause()
    {
    }

    /// <inheritdoc />
    public override void Resume()
    {
    }

    /// <inheritdoc />
    public override void Resize( int width, int height )
    {
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        SpriteBatch?.Dispose();
        Background?.Dispose();
    }
}