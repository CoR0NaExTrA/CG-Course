using Silk.NET.OpenGL;

namespace AsteroidGame.Objects;

public class Bullet : GameObject
{
    public float Life = 2f;

    public override void Update( float dt )
    {
        Position += Velocity * dt;
        Life -= dt;
    }

    public override void Draw( Renderer renderer )
    {
        float[] v =
        {
            Position.X, Position.Y
        };

        renderer.Draw( v, PrimitiveType.Points );
    }
}