using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.Cameras;
using LughSharp.LibCore.Graphics.G2D;
using LughSharp.LibCore.Utils;

namespace ConsoleApp1.Source;

public class MainGame : ApplicationAdapter
{
    private OrthographicCamera? _camera;
    private SpriteBatch?        _spriteBatch;
    private Texture?            _background;

    /// <inheritdoc />
    public override void Create()
    {
        _spriteBatch = new SpriteBatch();

        _camera = new OrthographicCamera();
        _camera.SetToOrtho( false, 640, 480 );

        _background = new Texture( Gdx.Files.Internal( "LughLogo.png" ).FileName );
    }

    /// <inheritdoc />
    public override void Render()
    {
        ScreenUtils.Clear( 0, 0, 0.2f, 1 );
        
        _camera?.Update();
        
        _spriteBatch?.SetProjectionMatrix( _camera!.Combined );
        _spriteBatch?.Begin();
        _spriteBatch?.Draw( _background!, 0, 0 );
        _spriteBatch?.End();
    }

//    /// <inheritdoc />
//    public override void Pause()
//    {
//    }

//    /// <inheritdoc />
//    public override void Resume()
//    {
//    }
}