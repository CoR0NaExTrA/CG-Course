using Silk.NET.OpenGL;

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
        uint vs = gl.CreateShader(GLEnum.VertexShader);
        gl.ShaderSource(vs, vertexSource);
        gl.CompileShader(vs);

        uint fs = gl.CreateShader(GLEnum.FragmentShader);
        gl.ShaderSource(fs, fragmentSource);
        gl.CompileShader(fs);

        Program = gl.CreateProgram();
        gl.AttachShader(Program, vs);
        gl.AttachShader(Program, fs);
        gl.LinkProgram(Program);

        gl.DeleteShader(vs);
        gl.DeleteShader(fs);
    }

    public void Use() => gl.UseProgram(Program);

    public void SetMatrix(string name, System.Numerics.Matrix4x4 mat)
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
}