using Silk.NET.OpenGL;

namespace Maze3D.Rendering;

public static class Geometry
{
    public static readonly float[] CubeVertices =
    {
        -0.5f,-0.5f,-0.5f,  0.0f, 0.0f,
         0.5f,-0.5f,-0.5f,  1.0f, 0.0f,
         0.5f, 0.5f,-0.5f,  1.0f, 1.0f,
         0.5f, 0.5f,-0.5f,  1.0f, 1.0f,
        -0.5f, 0.5f,-0.5f,  0.0f, 1.0f,
        -0.5f,-0.5f,-0.5f,  0.0f, 0.0f,

        -0.5f,-0.5f, 0.5f,  0.0f, 0.0f,
         0.5f,-0.5f, 0.5f,  1.0f, 0.0f,
         0.5f, 0.5f, 0.5f,  1.0f, 1.0f,
         0.5f, 0.5f, 0.5f,  1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f,  0.0f, 1.0f,
        -0.5f,-0.5f, 0.5f,  0.0f, 0.0f,

        -0.5f, 0.5f, 0.5f,  1.0f, 0.0f,
        -0.5f, 0.5f,-0.5f,  1.0f, 1.0f,
        -0.5f,-0.5f,-0.5f,  0.0f, 1.0f,
        -0.5f,-0.5f,-0.5f,  0.0f, 1.0f,
        -0.5f,-0.5f, 0.5f,  0.0f, 0.0f,
        -0.5f, 0.5f, 0.5f,  1.0f, 0.0f,

         0.5f, 0.5f, 0.5f,  0.0f, 0.0f,
         0.5f, 0.5f,-0.5f,  1.0f, 0.0f,
         0.5f,-0.5f,-0.5f,  1.0f, 1.0f,
         0.5f,-0.5f,-0.5f,  1.0f, 1.0f,
         0.5f,-0.5f, 0.5f,  0.0f, 1.0f,
         0.5f, 0.5f, 0.5f,  0.0f, 0.0f,

        -0.5f,-0.5f,-0.5f,  0.0f, 1.0f,
         0.5f,-0.5f,-0.5f,  1.0f, 1.0f,
         0.5f,-0.5f, 0.5f,  1.0f, 0.0f,
         0.5f,-0.5f, 0.5f,  1.0f, 0.0f,
        -0.5f,-0.5f, 0.5f,  0.0f, 0.0f,
        -0.5f,-0.5f,-0.5f,  0.0f, 1.0f,

        -0.5f, 0.5f,-0.5f,  0.0f, 0.0f,
         0.5f, 0.5f,-0.5f,  1.0f, 0.0f,
         0.5f, 0.5f, 0.5f,  1.0f, 1.0f,
         0.5f, 0.5f, 0.5f,  1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f,  0.0f, 1.0f,
        -0.5f, 0.5f,-0.5f,  0.0f, 0.0f
    };

    public const int VertexCount = 36;
    public const int VertexStride = 5;
    public const int PositionOffset = 0;
    public const int UVOffset = 3;

    public unsafe static (uint vao, uint vbo) CreateCube(GL gl)
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

        gl.VertexAttribPointer(0, 3, GLEnum.Float, false,
            VertexStride * sizeof(float), (void*)(PositionOffset * sizeof(float)));
        gl.EnableVertexAttribArray(0);

        gl.VertexAttribPointer(1, 2, GLEnum.Float, false,
            VertexStride * sizeof(float), (void*)(UVOffset * sizeof(float)));
        gl.EnableVertexAttribArray(1);

        return (vao, vbo);
    }
}