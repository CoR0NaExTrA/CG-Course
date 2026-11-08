using Silk.NET.OpenGL;
using System.Numerics;

namespace Maze3D.Rendering;

public class ShaderManager
{
    public uint Program { get; private set; }
    private readonly GL gl;

    public ShaderManager(GL gl)
    {
        this.gl = gl;
    }

    public void Load(string vertexSource, string fragmentSource)
    {
        uint vs = CompileShader(GLEnum.VertexShader, vertexSource);
        uint fs = CompileShader(GLEnum.FragmentShader, fragmentSource);

        Program = gl.CreateProgram();
        gl.AttachShader(Program, vs);
        gl.AttachShader(Program, fs);
        gl.LinkProgram(Program);

        gl.DetachShader(Program, vs);
        gl.DetachShader(Program, fs);
        gl.DeleteShader(vs);
        gl.DeleteShader(fs);

        gl.GetProgram(Program, GLEnum.LinkStatus, out var status);
        if (status == 0)
        {
            string info = gl.GetProgramInfoLog(Program);
            throw new Exception($"❌ Ошибка линковки шейдеров: {info}");
        }
    }

    private uint CompileShader(GLEnum type, string source)
    {
        uint shader = gl.CreateShader(type);
        gl.ShaderSource(shader, source);
        gl.CompileShader(shader);

        gl.GetShader(shader, GLEnum.CompileStatus, out var status);
        if (status == 0)
        {
            string info = gl.GetShaderInfoLog(shader);
            gl.DeleteShader(shader);
            throw new Exception($"❌ Ошибка компиляции шейдера: {info}");
        }
        return shader;
    }

    public void Use() => gl.UseProgram(Program);

    public void SetMatrix(string name, Matrix4x4 mat)
    {
        int loc = gl.GetUniformLocation(Program, name);
        unsafe
        {
            gl.UniformMatrix4(loc, 1, false, (float*)&mat);
        }
    }

    public void SetColor(string name, float r, float g, float b)
    {
        int loc = gl.GetUniformLocation(Program, name);
        gl.Uniform3(loc, r, g, b);
    }

    public void SetTexture(string name, int textureUnit)
    {
        int loc = gl.GetUniformLocation(Program, name);
        gl.Uniform1(loc, textureUnit);
    }

    public void SetFloat(string name, float value)
    {
        int loc = gl.GetUniformLocation(Program, name);
        gl.Uniform1(loc, value);
    }
}