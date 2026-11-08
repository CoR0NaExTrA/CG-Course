using Silk.NET.OpenGL;

namespace AsteroidGame;

public class GameShader
{
    private GL gl;
    public uint Handle;

    public GameShader( GL gl, string vertexSource, string fragmentSource )
    {
        this.gl = gl;

        uint vertex = gl.CreateShader( ShaderType.VertexShader );
        gl.ShaderSource( vertex, vertexSource );
        gl.CompileShader( vertex );

        uint fragment = gl.CreateShader( ShaderType.FragmentShader );
        gl.ShaderSource( fragment, fragmentSource );
        gl.CompileShader( fragment );

        Handle = gl.CreateProgram();
        gl.AttachShader( Handle, vertex );
        gl.AttachShader( Handle, fragment );
        gl.LinkProgram( Handle );

        gl.DeleteShader( vertex );
        gl.DeleteShader( fragment );
    }

    public void Use()
    {
        gl.UseProgram( Handle );
    }
}
