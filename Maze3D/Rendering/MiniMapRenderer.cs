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
        int[] lastViewport = new int[4];
        gl.GetInteger(GLEnum.Viewport, lastViewport);

        gl.Viewport(viewportX, viewportY, (uint)size, (uint)size);

        shader.Use();

        Matrix4x4 view = Matrix4x4.Identity;
        Matrix4x4 proj = Matrix4x4.CreateOrthographicOffCenter(0, Constants.MAZE_SIZE, Constants.MAZE_SIZE, 0, -1, 1);

        shader.SetMatrix("view", view);
        shader.SetMatrix("projection", proj);

        gl.BindVertexArray(vao);

        for (int x = 0; x < Constants.MAZE_SIZE; x++)
        {
            for (int z = 0; z < Constants.MAZE_SIZE; z++)
            {
                WallType cell = maze[x, z];

                if (cell != WallType.None)
                {
                    shader.SetColor("color", 1f, 1f, 1f);
                }
                else
                {
                    shader.SetColor("color", 0.15f, 0.15f, 0.15f);
                }

                Matrix4x4 model = Matrix4x4.CreateScale(0.9f, 0.9f, 1f) *
                                 Matrix4x4.CreateTranslation(x, z, 0);
                shader.SetMatrix("model", model);

                shader.SetFloat("useTexture", 0);

                gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
            }
        }

        shader.SetColor("color", 0f, 1f, 0f);
        shader.SetFloat("useTexture", 0);

        Matrix4x4 playerModel =
            Matrix4x4.CreateScale(Constants.MINIMAP_PLAYER_SCALE, Constants.MINIMAP_PLAYER_SCALE, 1f) *
            Matrix4x4.CreateTranslation(player.Position.X, player.Position.Z, 0.02f);
        shader.SetMatrix("model", playerModel);
        gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);

        gl.Viewport(lastViewport[0], lastViewport[1], (uint)lastViewport[2], (uint)lastViewport[3]);
    }
}