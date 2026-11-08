using Silk.NET.OpenGL;
using System.Numerics;

namespace MobiusStrip;

class ShaderProgram
{
    private readonly GL _gl;
    public uint Handle { get; private set; }

    public ShaderProgram( GL gl ) => _gl = gl;

    public void Create()
    {
        uint vs = CompileShader( GLEnum.VertexShader, VertexSource );
        uint fs = CompileShader( GLEnum.FragmentShader, FragmentSource );

        Handle = _gl.CreateProgram();
        _gl.AttachShader( Handle, vs );
        _gl.AttachShader( Handle, fs );
        _gl.LinkProgram( Handle );

        _gl.DeleteShader( vs );
        _gl.DeleteShader( fs );
    }

    private uint CompileShader( GLEnum type, string source )
    {
        uint shader = _gl.CreateShader( type );
        _gl.ShaderSource( shader, source );
        _gl.CompileShader( shader );

        return shader;
    }

    public void Use() => _gl.UseProgram( Handle );

    public unsafe void SetMatrix4( string name, Matrix4x4 matrix )
    {
        int loc = _gl.GetUniformLocation( Handle, name );
        _gl.UniformMatrix4( loc, 1, false, ( float* )&matrix );
    }

    public void SetVector3( string name, Vector3 value )
    {
        int loc = _gl.GetUniformLocation( Handle, name );
        _gl.Uniform3( loc, value );
    }

    private const string VertexSource = @"
        #version 330 core
        layout (location=0) in vec3 aPos;
        layout (location=1) in vec3 aNormal;
        out vec3 FragPos;
        out vec3 Normal;
        uniform mat4 model, view, projection;
        
        void main()
        {
            FragPos = vec3(model * vec4(aPos, 1.0));
            Normal = mat3(transpose(inverse(model))) * aNormal;
            gl_Position = projection * view * vec4(FragPos, 1.0);
        }";

    private const string FragmentSource = @"
        #version 330 core
        in vec3 FragPos;
        in vec3 Normal;
        out vec4 FragColor;
        uniform vec3 lightPos;
        
        void main()
        {
            vec3 norm = normalize(Normal);

            if (dot(norm, normalize(-FragPos)) < 0.0)
                norm = -norm;

            vec3 lightDir = normalize(lightPos - FragPos);
            float diff = max(dot(norm, lightDir), 0.0);
            vec3 color = vec3(0.8, 0.3, 0.6);
            FragColor = vec4((0.2 + diff) * color, 1.0);
        }";
}