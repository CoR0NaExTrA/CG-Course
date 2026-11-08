using System.Numerics;

namespace AsteroidGame.Objects;

public abstract class GameObject
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;

    public abstract void Update( float dt );
    public abstract void Draw( Renderer renderer );
}
