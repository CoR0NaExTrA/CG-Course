using System.Numerics;

namespace Battleship3D.Models;

public class Torpedo : GameObject
{
    public Vector3 Direction;
    public float Speed = 25f;

    public Torpedo( Mesh mesh, uint texture, Vector3 pos, Vector3 dir )
    {
        Mesh = mesh;
        Texture = texture;
        Position = pos;
        Direction = Vector3.Normalize( dir );

        Scale = new Vector3( 1f );
    }

    public override void Update( float dt )
    {
        Position += Direction * Speed * dt;

        float yaw =
            MathF.Atan2( Direction.X, Direction.Z );

        Rotation.Y =
            yaw * 180f / MathF.PI;

        if ( Position.Length() > 200 )
            Active = false;
    }
}