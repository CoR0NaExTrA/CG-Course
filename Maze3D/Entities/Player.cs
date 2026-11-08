using System.Numerics;
using Maze3D.Core;
using Maze3D.World;

namespace Maze3D.Entities;

public class Player
{
    public Vector3 Position { get; set; }
    public float Radius => Constants.PLAYER_RADIUS;

    public Player(Vector3 startPosition)
    {
        Position = startPosition;
    }

    public void Move(Vector3 delta, Maze maze)
    {
        if (delta == Vector3.Zero) return;

        float maxStep = Radius * Constants.MAX_STEP_FACTOR;
        if (delta.Length() > maxStep)
            delta = Vector3.Normalize(delta) * maxStep;

        Vector3 proposed = Position + delta;

        bool canMoveX = !Collision.CheckWall(maze,
            new Vector3(proposed.X, Position.Y, Position.Z), Radius);
        bool canMoveZ = !Collision.CheckWall(maze,
            new Vector3(Position.X, Position.Y, proposed.Z), Radius);

        if (canMoveX && canMoveZ)
            Position = proposed;
        else if (canMoveX)
            Position = new Vector3(proposed.X, Position.Y, Position.Z);
        else if (canMoveZ)
            Position = new Vector3(Position.X, Position.Y, proposed.Z);

        Position = new Vector3(Position.X, Constants.PLAYER_Y, Position.Z);
    }
}