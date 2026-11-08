using System.Numerics;

namespace SnubDodecahedron.Models;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector3 Color;

    public Vertex( Vector3 pos, Vector3 normal, Vector3 color )
    {
        Position = pos;
        Normal = normal;
        Color = color;
    }
}
