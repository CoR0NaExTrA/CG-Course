using MemoryTrainer.Core;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace MemoryTrainer.Rendering;

public class Renderer
{
    private readonly IWindow _window;
    private readonly Game _game;

    private GL _gl = null!;
    private ShaderManager _shader = null!;
    private TextureLoader _textureLoader = null!;
    private MeshData _cubeMesh;

    private uint _frontTexture;
    private Dictionary<int, uint> _backTextures = new();

    public Camera Camera { get; } = new Camera(
        position: new Vector3( 6, 8, 10 ),
        target: new Vector3( 2.5f, 0, 2.5f ),
        up: Vector3.UnitY,
        fov: MathF.PI / 4f,
        near: 0.1f,
        far: 100f );

    public Renderer( IWindow window, Game game )
    {
        _window = window;
        _game = game;
    }

    public void Load()
    {
        _gl = GL.GetApi( _window );
        _gl.Enable( GLEnum.DepthTest );

        _cubeMesh = MeshGenerator.CreateCube( _gl, 1f, 0.2f );

        _shader = new ShaderManager( _gl,
            "Shaders/cube.vert",
            "Shaders/cube.frag" );

        _textureLoader = new TextureLoader( _gl );

        _frontTexture = _textureLoader.Load( "textures/front.png" );

        for ( int i = 0; i < _game.TotalPairs; i++ )
            _backTextures[ i ] = _textureLoader.Load( $"textures/card{i}.png" );

        _shader.Use();
        _shader.SetInt( "tex", 0 );
    }

    public void Render( double dt )
    {
        _gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

        _shader.Use();
        SetupMatrices();
        SetupLighting();

        foreach ( var tile in _game.GetAllTiles() )
        {
            if ( tile.IsMatched )
                continue;

            RenderTile( tile );
        }
    }

    private void SetupMatrices()
    {
        var aspect = _window.Size.X / ( float )_window.Size.Y;
        Camera.UpdateProjection( aspect );

        _shader.SetMatrix( "view", Camera.ViewMatrix );
        _shader.SetMatrix( "projection", Camera.ProjectionMatrix );
    }

    private void SetupLighting()
    {
        var lightPos = new Vector3( 5, 10, 5 );
        var viewPos = Camera.Position;

        _shader.SetVector3( "lightPos", lightPos );
        _shader.SetVector3( "viewPos", viewPos );
    }

    private void RenderTile( Tile tile )
    {
        _shader.SetMatrix( "model", tile.GetModelMatrix() );

        bool showBack = tile.Rotation >= MathF.PI / 2f;
        _shader.SetInt( "isBack", showBack ? 1 : 0 );

        uint texture = showBack ? _backTextures[ tile.Id ] : _frontTexture;

        _gl.ActiveTexture( GLEnum.Texture0 );
        _gl.BindTexture( GLEnum.Texture2D, texture );

        _cubeMesh.Bind( _gl );
        _gl.DrawArrays( GLEnum.Triangles, 0, (uint)_cubeMesh.VertexCount );
    }
}