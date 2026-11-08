using Silk.NET.OpenGL;

namespace Meadow.Graphics;

public class MeadowShader: IDisposable
{
    private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec3 aColor;

        out vec3 ourColor;

        void main()
        {
            gl_Position = vec4(aPos, 1.0);
            ourColor = aColor;
        }
        ";

    private static readonly string FragmentShaderSource = @"
        #version 330 core
        in vec3 ourColor;
        out vec4 FragColor;

        void main()
        {
            FragColor = vec4(ourColor, 1.0);
        }
        ";

    public uint Handle { get; }
    private readonly GL _gl;

    public MeadowShader( GL gl )
    {
        _gl = gl;
        Handle = CompileProgram( VertexShaderSource, FragmentShaderSource );
    }

    private uint CompileProgram( string vs, string fs )
    {
        uint vertexShader = _gl.CreateShader( ShaderType.VertexShader );
        _gl.ShaderSource( vertexShader, VertexShaderSource );
        _gl.CompileShader( vertexShader );
        CheckShaderCompile( _gl, vertexShader, "Vertex" );

        uint fragmentShader = _gl.CreateShader( ShaderType.FragmentShader );
        _gl.ShaderSource( fragmentShader, FragmentShaderSource );
        _gl.CompileShader( fragmentShader );
        CheckShaderCompile( _gl, fragmentShader, "Fragment" );

        uint program = _gl.CreateProgram();
        _gl.AttachShader( program, vertexShader );
        _gl.AttachShader( program, fragmentShader );
        _gl.LinkProgram( program );
        CheckProgramLink( _gl, program );

        _gl.DetachShader( program, vertexShader );
        _gl.DetachShader( program, fragmentShader );
        _gl.DeleteShader( vertexShader );
        _gl.DeleteShader( fragmentShader );

        return program;
    }

    private static void CheckShaderCompile( GL gl, uint shader, string type )
    {
        string log = gl.GetShaderInfoLog( shader );
        if ( !string.IsNullOrWhiteSpace( log ) )
            Console.WriteLine( $"❌ Ошибка компиляции {type} шейдера: {log}" );
    }

    private static void CheckProgramLink( GL gl, uint program )
    {
        gl.GetProgram( program, GLEnum.LinkStatus, out var status );
        if ( status == 0 )
            Console.WriteLine( $"❌ Ошибка линковки программы: {gl.GetProgramInfoLog( program )}" );
    }

    public void Use() => _gl.UseProgram( Handle );

    public void Dispose() => _gl.DeleteProgram( Handle );
}
