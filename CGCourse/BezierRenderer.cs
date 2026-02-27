using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CGCourse;

internal sealed class BezierRenderer : IDisposable
{
    private readonly SceneGeometry _geometry;
    private readonly int _shaderProgram;
    private readonly int _vao;

    public BezierRenderer(SceneGeometry geometry)
    {
        _geometry = geometry;
        _shaderProgram = ShaderFactory.CreateDefaultProgram();
        _vao = GL.GenVertexArray();
    }

    public void Render(Matrix4 projection)
    {
        DrawLines(_geometry.AxesVertices, PrimitiveType.Lines, SceneConfig.AxesColor, projection, 2.4f);
        DrawLines(_geometry.DashedControlVertices, PrimitiveType.Lines, SceneConfig.ControlPolygonColor, projection, 1.8f);
        DrawLines(_geometry.BezierVertices, PrimitiveType.LineStrip, SceneConfig.GraphColor, projection, 3.0f);
        DrawPoints(_geometry.ControlPointVertices, SceneConfig.ControlPointColor, projection, 11f);
    }

    public void Dispose()
    {
        if (_vao != 0)
        {
            GL.DeleteVertexArray(_vao);
        }

        if (_shaderProgram != 0)
        {
            GL.DeleteProgram(_shaderProgram);
        }
    }

    private void DrawLines(float[] vertices, PrimitiveType primitiveType, Vector3 color, Matrix4 projection, float lineWidth)
    {
        using var vbo = new BufferObject(vertices);
        DrawBuffer(vbo.Handle, primitiveType, vertices.Length / 2, color, projection, lineWidth, pointSize: 1f);
    }

    private void DrawPoints(float[] vertices, Vector3 color, Matrix4 projection, float pointSize)
    {
        using var vbo = new BufferObject(vertices);
        DrawBuffer(vbo.Handle, PrimitiveType.Points, vertices.Length / 2, color, projection, lineWidth: 1f, pointSize: pointSize);
    }

    private void DrawBuffer(int vboHandle, PrimitiveType primitiveType, int vertexCount, Vector3 color, Matrix4 projection, float lineWidth, float pointSize)
    {
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.UseProgram(_shaderProgram);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "uProjection"), false, ref projection);
        GL.Uniform3(GL.GetUniformLocation(_shaderProgram, "uColor"), color);
        GL.Uniform1(GL.GetUniformLocation(_shaderProgram, "uPointSize"), pointSize);

        GL.LineWidth(lineWidth);
        GL.DrawArrays(primitiveType, 0, vertexCount);
        GL.DisableVertexAttribArray(0);
    }
}
