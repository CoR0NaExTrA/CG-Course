using System.Numerics;
using Maze3D.Core;

namespace Maze3D.World;

public static class Collision
{
    public static bool CheckWall(Maze maze, Vector3 pos, float radius)
    {
        float checkRadius = radius + Constants.COLLISION_MARGIN;
        float[] offsets = { -checkRadius, 0f, +checkRadius };

        foreach (float ox in offsets)
        {
            foreach (float oz in offsets)
            {
                int x = ( int )MathF.Floor( pos.X + ox + 0.5f );
                int z = ( int )MathF.Floor( pos.Z + oz + 0.5f );

                if (maze.IsWall(x, z))
                    return true;
            }
        }
        return false;
    }
}