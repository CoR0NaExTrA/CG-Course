using System.Numerics;
using Maze3D.Core;
using Maze3D.Utilities;

namespace Maze3D.Entities;

public class Camera
{
    public Vector3 Position { get; set; }
    public float Yaw { get; set; } = -90f;
    public float Pitch { get; set; }

    public Camera(Vector3 startPosition)
    {
        Position = startPosition;
    }

    public Vector3 Front => MathHelper.GetFront(Yaw, Pitch);

    public Matrix4x4 GetViewMatrix() =>
        Matrix4x4.CreateLookAt(Position, Position + Front, Vector3.UnitY);

    public Matrix4x4 GetProjectionMatrix(float aspectRatio) =>
        Matrix4x4.CreatePerspectiveFieldOfView(
            Constants.FOV, aspectRatio, Constants.NEAR_PLANE, Constants.FAR_PLANE);

    public void Rotate(float deltaX, float deltaY)
    {
        Yaw += deltaX * Constants.MOUSE_SENSITIVITY;
        Pitch -= deltaY * Constants.MOUSE_SENSITIVITY;
        Pitch = Math.Clamp(Pitch, Constants.PITCH_MIN, Constants.PITCH_MAX);
    }

    public void UpdateFromPlayer(Player player)
    {
        Position = new Vector3(player.Position.X, player.Position.Y, player.Position.Z);
    }
}