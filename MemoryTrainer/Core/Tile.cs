using MemoryTrainer.Utils;
using System.Numerics;

namespace MemoryTrainer.Core;

public class Tile
{
    public int Id { get; set; }
    public Vector3 Position { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsMatched { get; set; }

    public float Rotation { get; private set; } = 0f;
    public float TargetRotation => IsRevealed ? MathF.PI : 0f;
    public bool IsAnimating => MathF.Abs( Rotation - TargetRotation ) > 0.01f;

    public void Update( float deltaTime, float speed = 6f )
    {
        Rotation = MathHelper.Lerp( Rotation, TargetRotation, deltaTime * speed );
    }

    public Matrix4x4 GetModelMatrix()
    {
        return Matrix4x4.CreateTranslation( -0.5f, -0.1f, -0.5f ) *
               Matrix4x4.CreateRotationX( Rotation ) *
               Matrix4x4.CreateTranslation( Position + new Vector3( 0.5f, 0.1f, 0.5f ) );
    }

    public bool ContainsPoint( Vector3 point )
    {
        var min = Position;
        var max = Position + new Vector3( 1f, 0.2f, 1f );
        return point.X >= min.X && point.X <= max.X &&
               point.Y >= min.Y && point.Y <= max.Y &&
               point.Z >= min.Z && point.Z <= max.Z;
    }
}
