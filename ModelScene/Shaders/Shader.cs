using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Runtime.CompilerServices;
using System.Text;

namespace Task1.Shaders;

public class ChessShader
{
    private readonly GL _gl;
    private readonly uint _handle;

    public ChessShader( GL gl,
        string vertexPath = "../../../Shaders/shader.vert",
        string fragmentPath = "../../../Shaders/shader.frag" )
    {
        _gl = gl;

        string vertexShaderSource = File.ReadAllText( vertexPath );
        string fragmentShaderSource = File.ReadAllText( fragmentPath );

        uint vertexShader = _gl.CreateShader( ShaderType.VertexShader );
        _gl.ShaderSource( vertexShader, vertexShaderSource );
        _gl.CompileShader( vertexShader );
        CheckShaderCompileStatus( vertexShader, "Vertex" );

        uint fragmentShader = _gl.CreateShader( ShaderType.FragmentShader );
        _gl.ShaderSource( fragmentShader, fragmentShaderSource );
        _gl.CompileShader( fragmentShader );
        CheckShaderCompileStatus( fragmentShader, "Fragment" );

        _handle = _gl.CreateProgram();
        _gl.AttachShader( _handle, vertexShader );
        _gl.AttachShader( _handle, fragmentShader );
        _gl.LinkProgram( _handle );
        CheckProgramLinkStatus( _handle );

        _gl.DetachShader( _handle, vertexShader );
        _gl.DetachShader( _handle, fragmentShader );
        _gl.DeleteShader( vertexShader );
        _gl.DeleteShader( fragmentShader );
    }

    private void CheckShaderCompileStatus( uint shader, string type )
    {
        _gl.GetShader( shader, ShaderParameterName.CompileStatus, out int success );
        if ( success == 0 )
        {
            string infoLog = _gl.GetShaderInfoLog( shader );
            throw new ArgumentException( $"{type} shader compilation failed: {infoLog}" );
        }
    }

    private void CheckProgramLinkStatus( uint program )
    {
        _gl.GetProgram( program, ProgramPropertyARB.LinkStatus, out int success );
        if ( success == 0 )
        {
            string infoLog = _gl.GetProgramInfoLog( program );
            throw new ArgumentException( $"Program linking failed: {infoLog}" );
        }
    }

    public void Use() => _gl.UseProgram( _handle );

    public unsafe void SetMatrix4( string name, Matrix4X4<float> matrix )
    {
        int location = GetUniformLocation( name );
        _gl.UniformMatrix4( location, 1, false, ( float* )Unsafe.AsPointer( ref matrix ) );
    }

    public void SetVector3( string name, Vector3D<float> vector )
    {
        int location = GetUniformLocation( name );
        _gl.Uniform3( location, vector.X, vector.Y, vector.Z );
    }

    public void SetFloat( string name, float value )
    {
        int location = GetUniformLocation( name );
        _gl.Uniform1( location, value );
    }

    public void SetInt( string name, int value )
    {
        int location = GetUniformLocation( name );
        _gl.Uniform1( location, value );
    }

    public void SetBool( string name, bool value )
    {
        int location = GetUniformLocation( name );
        _gl.Uniform1( location, value ? 1 : 0 );
    }

    public int GetUniformLocation( string name )
    {
        return _gl.GetUniformLocation( _handle, Encoding.UTF8.GetBytes( name + "\0" ) );
    }
}