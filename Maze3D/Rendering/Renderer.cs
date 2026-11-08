using Maze3D.Entities;
using Maze3D.World;
using Maze3D.Core;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Maze3D.Rendering;

public class Renderer
{
    private readonly GL gl;
    private readonly uint vao, vbo;
    private readonly ShaderManager shader;

    public Renderer(GL gl)
    {
        this.gl = gl;
        shader = new ShaderManager(gl);
        LoadShaders();
        (vao, vbo) = Geometry.CreateCube(gl);
    }

    private void LoadShaders()
    {
        string vertex = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;
            uniform mat4 model, view, projection;
            void main() {
                gl_Position = projection * view * model * vec4(aPos, 1.0);
            }";

        string fragment = @"
            #version 330 core
            out vec4 FragColor;
            uniform vec3 color;
            void main() {
                FragColor = vec4(color, 1.0);
            }";

        shader.Load(vertex, fragment);
    }

    public void DrawWorld(Maze maze, Player player, Camera camera, float aspectRatio)
    {
        gl.ClearColor(0.1f, 0.1f, 0.15f, 1);
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

        shader.Use();
        shader.SetMatrix("view", camera.GetViewMatrix());
        shader.SetMatrix("projection", camera.GetProjectionMatrix(aspectRatio));

        gl.BindVertexArray(vao);

        shader.SetColor("color", 0.2f, 0.7f, 0.2f);
        for (int x = 0; x < Constants.MAZE_SIZE; x++)
        {
            for (int z = 0; z < Constants.MAZE_SIZE; z++)
            {
                Matrix4x4 model = Matrix4x4.CreateScale(1f, 0.05f, 1f) *
                                 Matrix4x4.CreateTranslation(x, -0.025f, z);
                shader.SetMatrix("model", model);
                gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
            }
        }

        for (int x = 0; x < Constants.MAZE_SIZE; x++)
        {
            for (int z = 0; z < Constants.MAZE_SIZE; z++)
            {
                if (maze[x, z] == 1)
                {
                    float shade = 0.5f + 0.5f * ((x + z) % 2);
                    shader.SetColor("color", 0.7f * shade, 0.2f * shade, 0.2f * shade);

                    Matrix4x4 model = Matrix4x4.CreateScale(1f, 2f, 1f) *
                                     Matrix4x4.CreateTranslation(x, 1f, z);
                    shader.SetMatrix("model", model);
                    gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
                }
            }
        }

        shader.SetColor("color", 0.2f, 0.2f, 0.7f);
        for (int x = 0; x < Constants.MAZE_SIZE; x++)
        {
            for (int z = 0; z < Constants.MAZE_SIZE; z++)
            {
                Matrix4x4 model = Matrix4x4.CreateScale(1f, 0.05f, 1f) *
                                 Matrix4x4.CreateTranslation(x, 2.025f, z);
                shader.SetMatrix("model", model);
                gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
            }
        }
    }

    public MiniMapRenderer CreateMiniMapRenderer() =>
        new MiniMapRenderer(gl, shader);
}