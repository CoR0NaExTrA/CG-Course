using System.Numerics;

namespace Battleship3D.Core;

public static class Collision
{
    public static bool Check( Vector3 a, Vector3 b, float dist )
    {
        return Vector3.Distance( a, b ) < dist;
    }
}