using Silk.NET.OpenGL;
using System.Numerics;

namespace AsteroidGame.Objects;

public class Ship : GameObject
{
    public float Thrust = 0;
    public float AngularVelocity;
    public bool EngineOn => Thrust > 0;

    public override void Update( float dt )
    {
        Rotation += AngularVelocity * dt;

        var direction = new Vector2(
            MathF.Cos( Rotation ),
            MathF.Sin( Rotation ) );

        Velocity += direction * Thrust * dt;

        Position += Velocity * dt;

        Velocity *= 0.99f;
        AngularVelocity *= 0.95f;
    }

    public override void Draw( Renderer renderer )
    {
        float size = 20;

        float[] vertices =
        {
            Position.X + MathF.Cos(Rotation) * size,
            Position.Y + MathF.Sin(Rotation) * size,

            Position.X + MathF.Cos(Rotation + 2.5f) * size,
            Position.Y + MathF.Sin(Rotation + 2.5f) * size,

            Position.X + MathF.Cos(Rotation - 2.5f) * size,
            Position.Y + MathF.Sin(Rotation - 2.5f) * size,
        };

        renderer.Draw( vertices, PrimitiveType.LineLoop );

        if ( EngineOn )
        {
            float flameSize = 10 + Random.Shared.NextSingle() * 10;

            float[] flame =
            {
            Position.X + MathF.Cos(Rotation - 3.0f) * flameSize,
            Position.Y + MathF.Sin(Rotation) * flameSize,

            Position.X + MathF.Cos(Rotation - 2.5f) * size * 0.8f,
            Position.Y + MathF.Sin(Rotation + 2.7f) * size * 0.8f,

            Position.X + MathF.Cos(Rotation + 2.5f) * size * 0.8f,
            Position.Y + MathF.Sin(Rotation - 2.7f) * size * 0.8f,
        };

            renderer.Draw( flame, PrimitiveType.LineLoop );
        }
    }
}
