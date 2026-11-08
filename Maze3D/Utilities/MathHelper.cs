using System.Numerics;

namespace Maze3D.Utilities;

public static class MathHelper
{
    public static Vector3 GetFront(float yaw, float pitch)
    {
        float yawRad = MathF.PI / 180 * yaw;
        float pitchRad = MathF.PI / 180 * pitch;

        return Vector3.Normalize(new Vector3(
            MathF.Cos(yawRad) * MathF.Cos(pitchRad),
            MathF.Sin(pitchRad),
            MathF.Sin(yawRad) * MathF.Cos(pitchRad)
        ));
    }

    public static Vector3 GetRight(Vector3 front) =>
        Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
}