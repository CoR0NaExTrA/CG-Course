using Silk.NET.OpenGL;
using System.Numerics;

namespace MemoryTrainer.Rendering;

public class ShaderManager
{
    private readonly GL _gl;
    public uint Program { get; }

    public ShaderManager( GL gl, string vertPath, string fragPath )
    {
        _gl = gl;
        Program = CreateProgram(
            File.ReadAllText( vertPath ),
            File.ReadAllText( fragPath ) );
    }

    private uint CreateProgram( string vertexSource, string fragmentSource )
    {
        uint vs = CompileShader( GLEnum.VertexShader, vertexSource );
        uint fs = CompileShader( GLEnum.FragmentShader, fragmentSource );

        uint program = _gl.CreateProgram();
        _gl.AttachShader( program, vs );
        _gl.AttachShader( program, fs );
        _gl.LinkProgram( program );

        _gl.DeleteShader( vs );
        _gl.DeleteShader( fs );

        return program;
    }

    private uint CompileShader( GLEnum type, string source )
    {
        uint shader = _gl.CreateShader( type );
        _gl.ShaderSource( shader, source );
        _gl.CompileShader( shader );
        return shader;
    }

    public void Use() => _gl.UseProgram( Program );

    public unsafe void SetMatrix( string name, Matrix4x4 mat )
    {
        int loc = _gl.GetUniformLocation( Program, name );
        _gl.UniformMatrix4( loc, 1, false, ( float* )&mat );
    }

    public void SetVector3( string name, Vector3 vec )
    {
        int loc = _gl.GetUniformLocation( Program, name );
        _gl.Uniform3( loc, vec.X, vec.Y, vec.Z );
    }

    public void SetInt( string name, int value )
    {
        int loc = _gl.GetUniformLocation( Program, name );
        _gl.Uniform1( loc, value );
    }
}