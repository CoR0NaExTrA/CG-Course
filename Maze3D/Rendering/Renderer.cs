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
    private readonly TextureManager textures;

    private readonly Dictionary<WallType, uint> wallTextures = new();

    public Renderer(GL gl)
    {
        this.gl = gl;
        shader = new ShaderManager(gl);
        textures = new TextureManager(gl);

        LoadShaders();
        LoadWallTextures();
        (vao, vbo) = Geometry.CreateCube(gl);
    }

    //разобраться в матрицах
    private void LoadShaders()
    {
        string vertex = @"
        #version 330 core
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec2 aUV;
        
        out vec2 TexCoord;
        
        uniform mat4 model, view, projection;
        
        void main() {
            gl_Position = projection * view * model * vec4(aPos, 1.0);
            TexCoord = aUV;
        }";

        string fragment = @"
        #version 330 core
        in vec2 TexCoord;
        out vec4 FragColor;
        
        uniform vec3 color;
        uniform sampler2D texture1;
        uniform bool useTexture;
        
        void main() {
            vec4 texColor = useTexture ? texture(texture1, TexCoord) : vec4(1.0);
            FragColor = texColor * vec4(color, 1.0);
        }";

        shader.Load(vertex, fragment);
    }

    private void LoadWallTextures()
    {
        foreach (WallType type in Enum.GetValues<WallType>())
        {
            if (type != WallType.None)
            {
                string path = type.GetTexturePath();
                wallTextures[type] = textures.LoadTexture(path);
            }
        }
    }

    public void DrawWorld(Maze maze, Player player, Camera camera, float aspectRatio)
    {
        gl.ClearColor(0.1f, 0.1f, 0.15f, 1);
        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

        shader.Use();
        shader.SetMatrix("view", camera.GetViewMatrix());
        shader.SetMatrix("projection", camera.GetProjectionMatrix(aspectRatio));

        gl.BindVertexArray(vao);
        gl.Enable(GLEnum.Texture2D);

        shader.SetColor("color", 0.2f, 0.7f, 0.2f);
        shader.SetTexture("texture1", 0);
        shader.SetFloat("useTexture", 0);

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
                WallType wallType = maze[x, z];
                if (wallType != WallType.None)
                {
                    if (wallTextures.TryGetValue(wallType, out uint texId))
                    {
                        textures.BindTexture(texId, 0);
                        shader.SetTexture("texture1", 0);
                        shader.SetFloat("useTexture", 1);
                    }

                    float shade = 0.8f + 0.2f * ((x + z) % 2);
                    shader.SetColor("color", shade, shade, shade);

                    Matrix4x4 model = Matrix4x4.CreateScale(1f, 2f, 1f) *
                                     Matrix4x4.CreateTranslation(x, 1f, z);
                    shader.SetMatrix("model", model);
                    gl.DrawArrays(GLEnum.Triangles, 0, Geometry.VertexCount);
                }
            }
        }

        shader.SetFloat("useTexture", 0);
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

        gl.Disable(GLEnum.Texture2D); 
    }

    public MiniMapRenderer CreateMiniMapRenderer() =>
        new MiniMapRenderer(gl, shader);
}