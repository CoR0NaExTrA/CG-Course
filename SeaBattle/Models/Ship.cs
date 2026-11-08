using System.Numerics;

namespace Battleship3D.Models;

public class Ship : GameObject
{
    public float Speed;
    public bool Sinking;

    public Ship( Mesh mesh, uint texture, Vector3 pos, float speed )
    {
        Mesh = mesh;
        Texture = texture;
        Position = pos;
        Speed = speed;

        Scale = new Vector3( 6f );

        Rotation.Y = -90f;
    }

    public override void Update( float dt )
    {
        if ( !Active )
            return;

        if ( Sinking )
        {
            Position.Y -= dt * 2f;

            if ( Position.Y < -10 )
                Active = false;

            return;
        }

        Position.X += Speed * dt;
    }
}