using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SnubDodecahedron.Graphics;
using SnubDodecahedron.Input;
using System.Numerics;

namespace SnubDodecahedron.Core;

public class WindowApp
{
    private IWindow window;
    private GL gl;

    private DodecahedronShader shader;
    private Mesh mesh;
    private CameraController camera = new();

    public WindowApp( IWindow window )
    {
        this.window = window;
        window.Load += OnLoad;
        window.Render += OnRender;
    }

    private void OnLoad()
    {
        gl = GL.GetApi( window );

        var input = window.CreateInput();
        if ( input.Mice.Count > 0 )
            camera.Attach( input.Mice[ 0 ] );

        gl.Enable( GLEnum.DepthTest );
        gl.Disable( GLEnum.CullFace );

        var vertices = Geometry.SnubDodecahedron.CreateVertices();
        var indices = Geometry.SnubDodecahedron.CreateIndices();

        mesh = new Mesh( gl, vertices, indices );

        shader = new DodecahedronShader( gl );
    }

    private void OnRender( double dt )
    {
        gl.ClearColor( 1, 1, 1, 1 );
        gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

        shader.Use();

        Matrix4x4 model = Matrix4x4.Identity;

        var cameraPos = camera.GetPosition();
        var lightPos = new Vector3( 20, 30, 20 );

        Matrix4x4 view = Matrix4x4.CreateLookAt( cameraPos, Vector3.Zero, Vector3.UnitY );
        Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView( MathF.PI / 4, 800f / 600f, 0.1f, 100f );

        SetMatrix( "model", model );
        SetMatrix( "view", view );
        SetMatrix( "projection", proj );
        gl.Uniform3( gl.GetUniformLocation( shader.GetProgram(), "viewPos" ), cameraPos );
        gl.Uniform3( gl.GetUniformLocation( shader.GetProgram(), "lightPos" ), lightPos );

        mesh.Draw( shader );
    }

    private unsafe void SetMatrix( string name, Matrix4x4 m )
    {
        int loc = gl.GetUniformLocation( shader.GetProgram(), name );
        gl.UniformMatrix4( loc, 1, false, ( float* )&m );
    }
}