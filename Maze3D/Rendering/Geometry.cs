using Silk.NET.OpenGL;

namespace Maze3D.Rendering;

public static class Geometry
{
    public static readonly float[] CubeVertices =
    {
        -0.5f,-0.5f,-0.5f,  0.5f,-0.5f,-0.5f,  0.5f, 0.5f,-0.5f,
         0.5f, 0.5f,-0.5f, -0.5f, 0.5f,-0.5f, -0.5f,-0.5f,-0.5f,
        -0.5f,-0.5f, 0.5f,  0.5f,-0.5f, 0.5f,  0.5f, 0.5f, 0.5f,
         0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f,-0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f, -0.5f, 0.5f,-0.5f, -0.5f,-0.5f,-0.5f,
        -0.5f,-0.5f,-0.5f, -0.5f,-0.5f, 0.5f, -0.5f, 0.5f, 0.5f,
         0.5f, 0.5f, 0.5f,  0.5f, 0.5f,-0.5f,  0.5f,-0.5f,-0.5f,
         0.5f,-0.5f,-0.5f,  0.5f,-0.5f, 0.5f,  0.5f, 0.5f, 0.5f,
        -0.5f,-0.5f,-0.5f,  0.5f,-0.5f,-0.5f,  0.5f,-0.5f, 0.5f,
         0.5f,-0.5f, 0.5f, -0.5f,-0.5f, 0.5f, -0.5f,-0.5f,-0.5f,
        -0.5f, 0.5f,-0.5f,  0.5f, 0.5f,-0.5f,  0.5f, 0.5f, 0.5f,
         0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f,-0.5f
    };

    public const int VertexCount = 36;
    public const int VertexStride = 3;

    public static (uint vao, uint vbo) CreateCube(GL gl)
    {
        uint vao = gl.GenVertexArray();
        uint vbo = gl.GenBuffer();

        gl.BindVertexArray(vao);
        gl.BindBuffer(GLEnum.ArrayBuffer, vbo);

        unsafe
        {
            fixed (float* v = CubeVertices)
            {
                gl.BufferData(GLEnum.ArrayBuffer,
                    (nuint)(CubeVertices.Length * sizeof(float)),
                    v, GLEnum.StaticDraw);
            }
        }

        gl.VertexAttribPointer(0, VertexStride, GLEnum.Float, false,
            VertexStride * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        return (vao, vbo);
    }
}