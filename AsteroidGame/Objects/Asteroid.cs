using Silk.NET.OpenGL;
using System.Numerics;

namespace AsteroidGame.Objects;

public class Asteroid : GameObject
{
    public int Size;

    Vector2[] shape;

    public Asteroid( Vector2 pos, int size )
    {
        Position = pos;
        Size = size;

        GenerateShape();
    }

    void GenerateShape()
    {
        int points = 10;
        shape = new Vector2[ points ];

        float radius = Size * 20;

        for ( int i = 0; i < points; i++ )
        {
            float angle = i / ( float )points * MathF.PI * 2;
            float r = radius + Random.Shared.Next( -10, 10 );

            shape[ i ] = new Vector2(
                MathF.Cos( angle ) * r,
                MathF.Sin( angle ) * r );
        }
    }

    public override void Update( float dt )
    {
        Position += Velocity * dt;
    }

    public override void Draw( Renderer renderer )
    {
        float[] vertices = new float[ shape.Length * 2 ];

        for ( int i = 0; i < shape.Length; i++ )
        {
            vertices[ i * 2 ] = Position.X + shape[ i ].X;
            vertices[ i * 2 + 1 ] = Position.Y + shape[ i ].Y;
        }

        renderer.Draw( vertices, PrimitiveType.LineLoop );
    }
}
