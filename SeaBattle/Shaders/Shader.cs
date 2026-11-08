using Silk.NET.OpenGL;
using System.Numerics;

namespace Battleship3D.Shaders;

public class Shader
{
    private GL gl;
    public uint Handle;

    public Shader( GL gl, string vertexSource, string fragmentSource )
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

    public unsafe void SetMatrix4( string name, System.Numerics.Matrix4x4 matrix )
    {
        int location = gl.GetUniformLocation( Handle, name );
        gl.UniformMatrix4( location, 1, false, ( float* )&matrix );
    }

    public void SetVector3( string name, Vector3 value )
    {
        int location = gl.GetUniformLocation( Handle, name );
        gl.Uniform3( location, value.X, value.Y, value.Z );
    }
}