using Silk.NET.OpenGL;

namespace SnubDodecahedron.Graphics;

public class DodecahedronShader
{
    private GL gl;

    private uint shaderProgram;

    private string vertex = @"
        #version 330 core

        layout(location = 0) in vec3 aPos;
        layout(location = 1) in vec3 aNormal;
        layout(location = 2) in vec3 aColor;

        out vec3 FragPos;
        out vec3 Normal;
        out vec3 Color;

        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;

        void main()
        {
            FragPos = vec3(model * vec4(aPos,1.0));
            Normal = mat3(transpose(inverse(model))) * aNormal;
            Color = aColor;

            gl_Position = projection * view * vec4(FragPos,1.0);
        }";

    private string fragment = @"
        #version 330 core

        in vec3 FragPos;
        in vec3 Normal;
        in vec3 Color;

        out vec4 FragColor;

        uniform vec3 lightPos;
        uniform vec3 viewPos;
        uniform bool isWireframe;

        void main()
        {
            if (isWireframe) {
                FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                return;
            }

            vec3 ambient = 0.4 * Color;

            vec3 norm = normalize(Normal);
            vec3 lightDir = normalize(lightPos - FragPos);

            float diff = max(dot(norm, lightDir), 0.0);
            vec3 diffuse = diff * Color;

            vec3 viewDir = normalize(viewPos - FragPos);
            vec3 reflectDir = reflect(-lightDir, norm);

            float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
            vec3 specular = vec3(0.4) * spec;

            vec3 result = ambient + diffuse + specular;

            FragColor = vec4(result, 1.0);
        }";

    public DodecahedronShader( GL gl )
    {
        this.gl = gl;
        shaderProgram = CreateShader();
    }

    public uint CreateShader()
    {
        uint vertexShader = gl.CreateShader( GLEnum.VertexShader );
        gl.ShaderSource( vertexShader, vertex );
        gl.CompileShader( vertexShader );

        uint fragmentShader = gl.CreateShader( GLEnum.FragmentShader );
        gl.ShaderSource( fragmentShader, fragment );
        gl.CompileShader( fragmentShader );

        uint program = gl.CreateProgram();
        gl.AttachShader( program, vertexShader );
        gl.AttachShader( program, fragmentShader );
        gl.LinkProgram( program );

        gl.DeleteShader( vertexShader );
        gl.DeleteShader( fragmentShader );

        return program;
    }

    public uint GetProgram() => shaderProgram;

    public void Use() => gl.UseProgram( shaderProgram );
}
