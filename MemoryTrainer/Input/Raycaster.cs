using MemoryTrainer.Core;
using MemoryTrainer.Rendering;
using Silk.NET.Maths;
using System.Numerics;

namespace MemoryTrainer.Input;

public readonly struct Ray
{
    public Vector3 Origin { get; }
    public Vector3 Direction { get; }

    public Ray( Vector3 origin, Vector3 direction )
    {
        Origin = origin;
        Direction = Vector3.Normalize( direction );
    }
}

public class Raycaster
{
    public Ray GetRay( float mouseX, float mouseY, Camera camera, Vector2D<int> viewport )
    {
        float x = ( 2f * mouseX ) / viewport.X - 1f;
        float y = 1f - ( 2f * mouseY ) / viewport.Y;

        var clip = new Vector4( x, y, -1f, 1f );

        Matrix4x4.Invert( camera.ProjectionMatrix, out var invProj );
        var eye = Vector4.Transform( clip, invProj );
        eye = new Vector4( eye.X, eye.Y, -1f, 0f );

        Matrix4x4.Invert( camera.ViewMatrix, out var invView );
        var world = Vector4.Transform( eye, invView );

        var dir = Vector3.Normalize( new Vector3( world.X, world.Y, world.Z ) );

        return new Ray( camera.Position, dir );
    }

    public bool IntersectsTile( Ray ray, Tile tile, out float distance )
    {
        distance = 0f;

        var min = tile.Position;
        var max = tile.Position + new Vector3( 1f, 0.2f, 1f );

        return RayIntersectsAABB( ray.Origin, ray.Direction, min, max, out distance );
    }

    private bool RayIntersectsAABB( Vector3 origin, Vector3 dir,
                                   Vector3 min, Vector3 max, out float t )
    {
        t = 0f;
        float tmin = float.MinValue;
        float tmax = float.MaxValue;

        for ( int i = 0; i < 3; i++ )
        {
            float o = origin[ i ];
            float d = dir[ i ];
            float mn = min[ i ];
            float mx = max[ i ];

            if ( MathF.Abs( d ) < 1e-8f )
            {
                if ( o < mn || o > mx )
                    return false;
                continue;
            }

            float t1 = ( mn - o ) / d;
            float t2 = ( mx - o ) / d;

            if ( t1 > t2 )
                (t1, t2) = (t2, t1);
            if ( t1 > tmin )
                tmin = t1;
            if ( t2 < tmax )
                tmax = t2;
            if ( tmin > tmax )
                return false;
        }

        t = tmin > 0 ? tmin : tmax;
        return t >= 0;
    }
}