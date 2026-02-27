using OpenTK.Graphics.OpenGL4;

namespace CGCourse;

internal static class ShaderFactory
{
    public static int CreateDefaultProgram()
    {
        const string vertexShaderSource = """
            #version 330 core

            layout (location = 0) in vec2 aPosition;

            uniform mat4 uProjection;
            uniform float uPointSize;

            void main()
            {
                gl_Position = uProjection * vec4(aPosition, 0.0, 1.0);
                gl_PointSize = uPointSize;
            }
            """;

        const string fragmentShaderSource = """
            #version 330 core

            out vec4 FragColor;
            uniform vec3 uColor;

            void main()
            {
                FragColor = vec4(uColor, 1.0);
            }
            """;

        var vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
        var fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

        var program = GL.CreateProgram();
        GL.AttachShader(program, vertexShader);
        GL.AttachShader(program, fragmentShader);
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var linkStatus);

        if (linkStatus == 0)
        {
            var info = GL.GetProgramInfoLog(program);
            throw new InvalidOperationException($"Ошибка линковки шейдеров: {info}");
        }

        GL.DetachShader(program, vertexShader);
        GL.DetachShader(program, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return program;
    }

    private static int CompileShader(ShaderType type, string source)
    {
        var shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);

        if (status == 0)
        {
            var info = GL.GetShaderInfoLog(shader);
            throw new InvalidOperationException($"Ошибка компиляции шейдера {type}: {info}");
        }

        return shader;
    }
}
