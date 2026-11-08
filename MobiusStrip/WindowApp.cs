using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Numerics;

namespace MobiusStrip;

class WindowApp
{
    private IWindow _window;
    private Camera _camera;
    private InputHandler _input;
    private MobiusMesh _mesh;
    private ShaderProgram _shader;
    private RenderContext _renderContext;

    public void Run()
    {
        var options = WindowOptions.Default with
        {
            Size = new Vector2D<int>( 1000, 700 ),
            Title = "Mobius Strip",
            API = new GraphicsAPI(
                ContextAPI.OpenGL,
                ContextProfile.Core,
                ContextFlags.Default,
                new APIVersion( 3, 3 ) )
        };

        _window = Window.Create( options );

        _window.Load += OnLoad;
        _window.Render += OnRender;

        _window.Run();
    }

    private void OnLoad()
    {
        _renderContext = new RenderContext( _window );
        _renderContext.Initialize();

        _camera = new Camera();
        _input = new InputHandler( _window, _camera );
        _input.Initialize();

        _mesh = new MobiusMesh( _renderContext.GL );
        _mesh.Generate( 200, 30 );
        _mesh.UploadToGPU();

        _shader = new ShaderProgram( _renderContext.GL );
        _shader.Create();
    }

    private void OnRender( double delta )
    {
        _renderContext.Clear();

        _shader.Use();
        SetupUniforms();

        _mesh.Draw();
    }

    private void SetupUniforms()
    {
        var aspect = _window.Size.X / ( float )_window.Size.Y;

        _shader.SetMatrix4( "model", _camera.ModelMatrix );
        _shader.SetMatrix4( "view", _camera.ViewMatrix );
        _shader.SetMatrix4( "projection",
            Matrix4x4.CreatePerspectiveFieldOfView(
                MathF.PI / 4, aspect, 0.1f, 100f ) );
        _shader.SetVector3( "lightPos", new( 3, 3, 3 ) );
    }
}
