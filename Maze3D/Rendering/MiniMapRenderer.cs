using Maze3D.Core;
using Maze3D.Entities;
using Maze3D.World;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Maze3D.Rendering;

public class MiniMapRenderer
{
    private readonly GL gl;
    private readonly uint vao, vbo;
    private readonly ShaderManager shader;

    public MiniMapRenderer(GL gl, ShaderManager shader)
    {
        this.gl = gl;
        this.shader = shader;
        (vao, vbo) = Geometry.CreateCube(gl);
    }

    public void Draw(Maze maze, Player player, Camera camera,
                    int viewportX, int viewportY, int size, int windowWidth, int windowHeight)
    {
        gl.Viewport(viewportX, viewportY, (uint)size, (uint)size);

        shader.Use();

        Matrix4x4 view = Matrix4x4.Identity;
        Matrix4x4 proj = Matrix4x4.CreateOrthographicOffCenter(
            0, Constants.MAZE_SIZE, 0, Constants.MAZE_SIZE, -1, 1);

        shader.SetMatrix("view", view);
        shader.SetMatrix("projection", proj);

        gl.BindVertexArray(vao);

        for (int x = 0; x < Constants.MAZE_SIZE; x++)
        {
            for (int z = 0; z < Constants.MAZE_SIZE; z++)
            {
                shader.SetColor("color",
                    maze[x, z] == 1 ? 1f : 0.15f,
                    maze[x, z] == 1 ? 1f : 0.15f,
                    maze[x, z] == 1 ? 1f : 0.15f);

                Matrix4x4 model = Matrix4x4.CreateTranslation(x, z, 0);
                shader.SetMatrix("model", model);
                gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
            }
        }

        shader.SetColor("color", 0f, 1f, 0f);
        Matrix4x4 playerModel =
            Matrix4x4.CreateScale(Constants.MINIMAP_PLAYER_SCALE, Constants.MINIMAP_PLAYER_SCALE, 1f) *
            Matrix4x4.CreateTranslation(player.Position.X, player.Position.Z, 0.02f);
        shader.SetMatrix("model", playerModel);
        gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);

        gl.Viewport(0, 0, (uint)windowWidth, (uint)windowHeight);
    }
}