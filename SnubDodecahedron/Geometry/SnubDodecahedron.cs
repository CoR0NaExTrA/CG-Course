using SnubDodecahedron.Models;
using System.Numerics;

namespace SnubDodecahedron.Geometry;

public static class SnubDodecahedron
{
    public static readonly int[][] Pentagons =
    {
        new[] { 0, 12, 48, 28, 36 }, new[] { 1, 13, 49, 29, 37 },
        new[] { 2, 14, 50, 30, 38 }, new[] { 3, 15, 51, 31, 39 },
        new[] { 4, 17, 53, 32, 40 }, new[] { 5, 16, 52, 33, 41 },
        new[] { 6, 19, 55, 34, 42 }, new[] { 7, 18, 54, 35, 43 },
        new[] { 8, 22, 58, 24, 44 }, new[] { 9, 23, 59, 25, 45 },
        new[] { 10, 20, 56, 26, 46 }, new[] { 11, 21, 57, 27, 47 }
    };

    public static readonly int[][] Triangles =
    {
        new[] { 0, 14, 2 }, new[] { 1, 15, 3 }, new[] { 2, 12, 0 }, new[] { 3, 13, 1 },
        new[] { 4, 16, 5 }, new[] { 5, 17, 4 }, new[] { 6, 18, 7 }, new[] { 7, 19, 6 },
        new[] { 8, 21, 11 }, new[] { 9, 20, 10 }, new[] { 10, 23, 9 }, new[] { 11, 22, 8 },
        new[] { 12, 56, 48 }, new[] { 13, 57, 49 }, new[] { 14, 58, 50 }, new[] { 15, 59, 51 },
        new[] { 16, 48, 52 }, new[] { 17, 49, 53 }, new[] { 18, 50, 54 }, new[] { 19, 51, 55 },
        new[] { 20, 52, 56 }, new[] { 21, 53, 57 }, new[] { 22, 54, 58 }, new[] { 23, 55, 59 },
        new[] { 24, 36, 44 }, new[] { 25, 37, 45 }, new[] { 26, 38, 46 }, new[] { 27, 39, 47 },
        new[] { 28, 40, 36 }, new[] { 29, 41, 37 }, new[] { 30, 42, 38 }, new[] { 31, 43, 39 },
        new[] { 32, 44, 40 }, new[] { 33, 45, 41 }, new[] { 34, 46, 42 }, new[] { 35, 47, 43 },
        new[] { 36, 24, 0 }, new[] { 37, 25, 1 }, new[] { 38, 26, 2 }, new[] { 39, 27, 3 },
        new[] { 40, 28, 4 }, new[] { 41, 29, 5 }, new[] { 42, 30, 6 }, new[] { 43, 31, 7 },
        new[] { 44, 32, 8 }, new[] { 45, 33, 9 }, new[] { 46, 34, 10 }, new[] { 47, 35, 11 },
        new[] { 48, 16, 28 }, new[] { 49, 17, 29 }, new[] { 50, 18, 30 }, new[] { 51, 19, 31 },
        new[] { 52, 20, 33 }, new[] { 53, 21, 32 }, new[] { 54, 22, 35 }, new[] { 55, 23, 34 },
        new[] { 56, 12, 26 }, new[] { 57, 13, 27 }, new[] { 58, 14, 24 }, new[] { 59, 15, 25 },
        new[] { 24, 14, 0 }, new[] { 25, 15, 1 }, new[] { 26, 12, 2 }, new[] { 27, 13, 3 },
        new[] { 28, 16, 4 }, new[] { 29, 17, 5 }, new[] { 30, 18, 6 }, new[] { 31, 19, 7 },
        new[] { 32, 21, 8 }, new[] { 33, 20, 9 }, new[] { 34, 23, 10 }, new[] { 35, 22, 11 },
        new[] { 36, 40, 44 }, new[] { 37, 41, 45 }, new[] { 38, 42, 46 }, new[] { 39, 43, 47 },
        new[] { 48, 56, 52 }, new[] { 49, 57, 53 }, new[] { 50, 58, 54 }, new[] { 51, 59, 55 }
    };

    public static Vertex[] CreateVertices()
    {
        float phi = ( 1f + MathF.Sqrt( 5f ) ) / 2f;
        float discriminant = phi - 5f / 27f;
        float sqrtDisc = MathF.Sqrt( discriminant );
        float x = MathF.Cbrt( ( phi + sqrtDisc ) / 2f ) + MathF.Cbrt( ( phi - sqrtDisc ) / 2f );

        float[] C =
        [
            phi * MathF.Sqrt( 3f - x * x ) / 2f,
            x * phi * MathF.Sqrt( 3f - x * x ) / 2f,
            phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
            x * x * phi * MathF.Sqrt( 3f - x * x ) / 2f,
            x * phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
            phi * MathF.Sqrt( 1f - x + ( 1f + phi ) / x ) / 2f,
            phi * MathF.Sqrt( x + 1f - phi ) / 2f,
            x * x * phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
            x * phi * MathF.Sqrt( 1f - x + ( 1f + phi ) / x ) / 2f,
            MathF.Sqrt( ( x + 2f ) * phi + 2f ) / 2f,
            x * MathF.Sqrt( x * ( 1f + phi ) - phi ) / 2f,
            MathF.Sqrt( x * x * ( 1f + 2f * phi ) - phi ) / 2f,
            phi * MathF.Sqrt( x * x + x ) / 2f,
            phi * phi * MathF.Sqrt( x * ( x + phi ) + 1f ) / ( 2f * x ),
            phi * MathF.Sqrt( x * ( x + phi ) + 1f ) / 2f,
        ];

        Vector3[] V =
        [
            new Vector3( C[ 2 ], C[ 1 ], C[ 14 ] ),
            new Vector3( C[ 2 ], -C[ 1 ], -C[ 14 ] ),
            new Vector3( -C[ 2 ], -C[ 1 ], C[ 14 ] ),
            new Vector3( -C[ 2 ], C[ 1 ], -C[ 14 ] ),
            new Vector3( C[ 14 ], C[ 2 ], C[ 1 ] ),
            new Vector3( C[ 14 ], -C[ 2 ], -C[ 1 ] ),
            new Vector3( -C[ 14 ], -C[ 2 ], C[ 1 ] ),
            new Vector3( -C[ 14 ], C[ 2 ], -C[ 1 ] ),
            new Vector3( C[ 1 ], C[ 14 ], C[ 2 ] ),
            new Vector3( C[ 1 ], -C[ 14 ], -C[ 2 ] ),
            new Vector3( -C[ 1 ], -C[ 14 ], C[ 2 ] ),
            new Vector3( -C[ 1 ], C[ 14 ], -C[ 2 ] ),

            new Vector3( C[ 3 ], -C[ 4 ], C[ 13 ] ),
            new Vector3( C[ 3 ], C[ 4 ], -C[ 13 ] ),
            new Vector3( -C[ 3 ], C[ 4 ], C[ 13 ] ),
            new Vector3( -C[ 3 ], -C[ 4 ], -C[ 13 ] ),
            new Vector3( C[ 13 ], -C[ 3 ], C[ 4 ] ),
            new Vector3( C[ 13 ], C[ 3 ], -C[ 4 ] ),
            new Vector3( -C[ 13 ], C[ 3 ], C[ 4 ] ),
            new Vector3( -C[ 13 ], -C[ 3 ], -C[ 4 ] ),
            new Vector3( C[ 4 ], -C[ 13 ], C[ 3 ] ),
            new Vector3( C[ 4 ], C[ 13 ], -C[ 3 ] ),
            new Vector3( -C[ 4 ], C[ 13 ], C[ 3 ] ),
            new Vector3( -C[ 4 ], -C[ 13 ], -C[ 3 ] ),

            new Vector3( C[ 0 ], C[ 8 ], C[ 12 ] ),
            new Vector3( C[ 0 ], -C[ 8 ], -C[ 12 ] ),
            new Vector3( -C[ 0 ], -C[ 8 ], C[ 12 ] ),
            new Vector3( -C[ 0 ], C[ 8 ], -C[ 12 ] ),
            new Vector3( C[ 12 ], C[ 0 ], C[ 8 ] ),
            new Vector3( C[ 12 ], -C[ 0 ], -C[ 8 ] ),
            new Vector3( -C[ 12 ], -C[ 0 ], C[ 8 ] ),
            new Vector3( -C[ 12 ], C[ 0 ], -C[ 8 ] ),
            new Vector3( C[ 8 ], C[ 12 ], C[ 0 ] ),
            new Vector3( C[ 8 ], -C[ 12 ], -C[ 0 ] ),
            new Vector3( -C[ 8 ], -C[ 12 ], C[ 0 ] ),
            new Vector3( -C[ 8 ], C[ 12 ], -C[ 0 ] ),

            new Vector3( C[ 7 ], C[ 6 ], C[ 11 ] ),
            new Vector3( C[ 7 ], -C[ 6 ], -C[ 11 ] ),
            new Vector3( -C[ 7 ], -C[ 6 ], C[ 11 ] ),
            new Vector3( -C[ 7 ], C[ 6 ], -C[ 11 ] ),
            new Vector3( C[ 11 ], C[ 7 ], C[ 6 ] ),
            new Vector3( C[ 11 ], -C[ 7 ], -C[ 6 ] ),
            new Vector3( -C[ 11 ], -C[ 7 ], C[ 6 ] ),
            new Vector3( -C[ 11 ], C[ 7 ], -C[ 6 ] ),
            new Vector3( C[ 6 ], C[ 11 ], C[ 7 ] ),
            new Vector3( C[ 6 ], -C[ 11 ], -C[ 7 ] ),
            new Vector3( -C[ 6 ], -C[ 11 ], C[ 7 ] ),
            new Vector3( -C[ 6 ], C[ 11 ], -C[ 7 ] ),

            new Vector3( C[ 9 ], -C[ 5 ], C[ 10 ] ),
            new Vector3( C[ 9 ], C[ 5 ], -C[ 10 ] ),
            new Vector3( -C[ 9 ], C[ 5 ], C[ 10 ] ),
            new Vector3( -C[ 9 ], -C[ 5 ], -C[ 10 ] ),
            new Vector3( C[ 10 ], -C[ 9 ], C[ 5 ] ),
            new Vector3( C[ 10 ], C[ 9 ], -C[ 5 ] ),
            new Vector3( -C[ 10 ], C[ 9 ], C[ 5 ] ),
            new Vector3( -C[ 10 ], -C[ 9 ], -C[ 5 ] ),
            new Vector3( C[ 5 ], -C[ 10 ], C[ 9 ] ),
            new Vector3( C[ 5 ], C[ 10 ], -C[ 9 ] ),
            new Vector3( -C[ 5 ], C[ 10 ], C[ 9 ] ),
            new Vector3( -C[ 5 ], -C[ 10 ], -C[ 9 ] ),
        ];

        Vertex[] vertices = new Vertex[ 60 ];

        Vector3 pentagonColor = new Vector3( 0.2f, 0.6f, 0.9f );
        Vector3 triangleColor = new Vector3( 0.9f, 0.4f, 0.2f );

        for ( int i = 0; i < 60; i++ )
        {
            Vector3 normal = Vector3.Normalize( V[ i ] );
            Vector3 color = ( i < 12 ) ? pentagonColor : triangleColor;

            vertices[ i ] = new Vertex( V[ i ], normal, color );
        }

        return vertices;
    }

    public static uint[] CreateIndices()
    {
        var indices = new List<uint>();

        foreach ( var p in Pentagons )
        {
            for ( int i = 1; i < 4; i++ )
            {
                indices.Add( ( uint )p[ 0 ] );
                indices.Add( ( uint )p[ i ] );
                indices.Add( ( uint )p[ i + 1 ] );
            }
        }

        foreach ( var t in Triangles )
        {
            indices.Add( ( uint )t[ 0 ] );
            indices.Add( ( uint )t[ 1 ] );
            indices.Add( ( uint )t[ 2 ] );
        }

        return indices.ToArray();
    }

    public static uint[] CreateEdgeIndices()
    {
        var edges = new HashSet<(uint, uint)>();

        void AddEdge( uint a, uint b )
        {
            if ( a > b )
                (a, b) = (b, a);
            edges.Add( (a, b) );
        }

        foreach ( var p in Pentagons )
        {
            for ( int i = 0; i < p.Length; i++ )
            {
                uint a = ( uint )p[ i ];
                uint b = ( uint )p[ ( i + 1 ) % p.Length ];
                AddEdge( a, b );
            }
        }

        foreach ( var t in Triangles )
        {
            AddEdge( ( uint )t[ 0 ], ( uint )t[ 1 ] );
            AddEdge( ( uint )t[ 1 ], ( uint )t[ 2 ] );
            AddEdge( ( uint )t[ 2 ], ( uint )t[ 0 ] );
        }

        return edges.SelectMany( e => new[] { e.Item1, e.Item2 } ).ToArray();
    }
}