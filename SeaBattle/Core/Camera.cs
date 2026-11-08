using System.Numerics;

namespace Battleship3D.Core;

public class Camera
{
    public Vector3 Position = new( 0, 25, 55 );
    public float Pitch = -20f;
    public float Yaw = -90f;

    public Matrix4x4 GetViewMatrix()
    {
        Vector3 front;

        front.X = MathF.Cos( MathF.PI / 180f * Pitch ) * MathF.Cos( MathF.PI / 180f * Yaw );
        front.Y = MathF.Sin( MathF.PI / 180f * Pitch );
        front.Z = MathF.Cos( MathF.PI / 180f * Pitch ) * MathF.Sin( MathF.PI / 180f * Yaw );

        front = Vector3.Normalize( front );

        return Matrix4x4.CreateLookAt( Position, Position + front, Vector3.UnitY );
    }
}