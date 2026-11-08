using Maze3D.Entities;
using Maze3D.Utilities;
using Silk.NET.Input;
using System.Numerics;

namespace Maze3D.Core;

public class InputHandler
{
    private readonly IKeyboard keyboard;
    private readonly IMouse mouse;

    private Vector2 lastMousePos;
    private bool firstMove = true;

    public InputHandler(IInputContext input)
    {
        keyboard = input.Keyboards[0];
        mouse = input.Mice[0];
        mouse.Cursor.CursorMode = CursorMode.Disabled;
    }

    public Vector2 GetMouseDelta()
    {
        var currentPos = mouse.Position;

        if (firstMove)
        {
            lastMousePos = currentPos;
            firstMove = false;
            return Vector2.Zero;
        }

        var delta = currentPos - lastMousePos;
        lastMousePos = currentPos;
        return delta;
    }

    public Vector3 GetMovementDirection(Camera camera)
    {
        Vector3 front = MathHelper.GetFront(camera.Yaw, camera.Pitch);
        front.Y = 0;
        front = Vector3.Normalize(front);
        Vector3 right = MathHelper.GetRight(front);

        Vector3 move = Vector3.Zero;

        if (keyboard.IsKeyPressed(Key.W)) move += front;
        if (keyboard.IsKeyPressed(Key.S)) move -= front;
        if (keyboard.IsKeyPressed(Key.A)) move -= right;
        if (keyboard.IsKeyPressed(Key.D)) move += right;

        return move != Vector3.Zero ? Vector3.Normalize(move) : Vector3.Zero;
    }

    public bool IsKeyPressed(Key key) => keyboard.IsKeyPressed(key);
}