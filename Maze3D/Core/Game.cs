using Maze3D.Entities;
using Maze3D.World;
using Maze3D.Rendering;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace Maze3D.Core;

public class Game
{
    private readonly IWindow window;
    private GL gl;

    private Maze maze;
    private Player player;
    private Camera camera;
    private InputHandler input;
    private Renderer renderer;
    private MiniMapRenderer miniMapRenderer;

    public Game(IWindow window)
    {
        this.window = window;
    }

    public void OnLoad()
    {
        gl = GL.GetApi(window);
        gl.Enable(GLEnum.DepthTest);

        maze = new Maze(Constants.MAZE_SIZE);
        maze.GenerateDFS();

        player = new Player( new Vector3( 1f, Constants.PLAYER_Y, 1f ) + new Vector3( 0.0f, 0, 0.0f ) );
        camera = new Camera(player.Position);

        input = new InputHandler(window.CreateInput());
        renderer = new Renderer(gl);
        miniMapRenderer = renderer.CreateMiniMapRenderer();
    }

    public void OnUpdate(double dt)
    {
        var mouseDelta = input.GetMouseDelta();
        camera.Rotate(mouseDelta.X, mouseDelta.Y);

        Vector3 moveDir = input.GetMovementDirection(camera);
        if (moveDir != Vector3.Zero)
        {
            player.Move(moveDir * Constants.MOVE_SPEED * (float)dt, maze);
            camera.UpdateFromPlayer(player);
        }
    }

    public void OnRender(double dt)
    {
        float aspect = window.Size.X / (float)window.Size.Y;

        renderer.DrawWorld(maze, player, camera, aspect);

        miniMapRenderer.Draw(maze, player, camera,
            0, 0, Constants.MINIMAP_SIZE,
            window.Size.X, window.Size.Y);
    }
}