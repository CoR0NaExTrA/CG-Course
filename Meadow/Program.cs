using Meadow.Graphics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Meadow;

class Program
{
    private static IWindow window;
    private static GL Gl;
    private static MeadowShader shader;
    private static Scene scene;

    private static void Main( string[] args )
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>( 800, 600 );
        options.Title = "Meadow";
        window = Window.Create( options );

        window.Load += OnLoad;
        window.Render += OnRender;
        window.FramebufferResize += OnResize;
        window.Closing += OnClose;

        window.Run();
        window.Dispose();
    }

    private static void OnLoad()
    {
        IInputContext input = window.CreateInput();
        for ( int i = 0; i < input.Keyboards.Count; i++ )
            input.Keyboards[ i ].KeyDown += KeyDown;

        Gl = GL.GetApi( window );
        Gl.ClearColor( 0.5f, 0.7f, 1.0f, 1.0f );

        shader = new MeadowShader( Gl );

        scene = new Scene( Gl );
    }

    private static void OnRender( double delta )
    {
        Gl.Clear( ( uint )ClearBufferMask.ColorBufferBit );
        shader.Use();
        scene.Render( Gl );
    }

    private static void OnResize( Vector2D<int> size )
    {
        Gl.Viewport( size );
    }

    private static void OnClose()
    {
        scene.Close();
        shader?.Dispose();
    }

    private static void KeyDown( IKeyboard kb, Key key, int scan )
    {
        if ( key == Key.Escape )
            window.Close();
    }
}