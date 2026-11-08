using System.Numerics;

namespace Battleship3D.Models;

public class GameObject
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale = Vector3.One;

    public Mesh Mesh;
    public uint Texture;

    public bool Active = true;

    public virtual void Update( float dt )
    {
    }

    public Matrix4x4 GetModelMatrix()
    {
        return
            Matrix4x4.CreateScale( Scale ) *
            Matrix4x4.CreateRotationX( MathF.PI / 180f * Rotation.X ) *
            Matrix4x4.CreateRotationY( MathF.PI / 180f * Rotation.Y ) *
            Matrix4x4.CreateRotationZ( MathF.PI / 180f * Rotation.Z ) *
            Matrix4x4.CreateTranslation( Position );
    }
}