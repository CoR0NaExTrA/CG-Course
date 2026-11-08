using Meadow.Generators;
using Meadow.Graphics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

public class Scene
{
    private GL Gl;
    private static GeometryGenerator geometryGenerator;

    private static List<Mesh2D> clouds = new();
    private static List<Mesh2D> flowers = new();
    private static List<Mesh2D> butterflies = new();
    private static Mesh2D grass;
    private static Mesh2D sun;
    private static Mesh2D meadow;

    private static readonly float[] Vertices =
    {
        -1.0f,  0.0f, 0.0f,   0.0f, 0.8f, 0.0f,
         1.0f,  0.0f, 0.0f,   0.0f, 0.8f, 0.0f,
         1.0f, -1.0f, 0.0f,   0.0f, 0.8f, 0.0f,

         1.0f, -1.0f, 0.0f,   0.0f, 0.8f, 0.0f,
        -1.0f, -1.0f, 0.0f,   0.0f, 0.8f, 0.0f,
        -1.0f,  0.0f, 0.0f,   0.0f, 0.8f, 0.0f
    };

    public Scene( GL gl )
    {
        Gl = gl;

        geometryGenerator = new GeometryGenerator();

        SetupRectangles( Gl );
        SetupSun( Gl );
        SetupClouds( Gl );
        SetupGrass( Gl );
        SetupFlowers( Gl );
        SetupButterflies( Gl );
    }

    private static void SetupRectangles( GL Gl )
    {
        meadow = new Mesh2D( Gl, Vertices );
    }

    private static void SetupSun( GL Gl )
    {
        var vertices = geometryGenerator.GenerateCircleVertices(
            0.7f, 0.7f, 0.15f,
            1f, 1f, 0.4f,
            1f, 0.6f, 0f,
            32
        );

        sun = new Mesh2D( Gl, vertices );
    }

    private static void SetupClouds( GL Gl )
    {
        var cloudPositions = new[]
        {
            new Vector2D<float>(-0.5f, 0.5f),
            new Vector2D<float>(0.2f, 0.8f),
            new Vector2D<float>(-0.8f, 0.2f)
        };

        foreach ( var pos in cloudPositions )
        {
            CreateCloud( Gl, pos.X, pos.Y );
        }
    }

    private static void CreateCloud( GL Gl, float centerX, float centerY )
    {
        var verts = new List<float>();

        var circles = new[]
        {
            new { x = centerX, y = centerY, r = 0.08f },
            new { x = centerX + 0.07f, y = centerY + 0.03f, r = 0.07f },
            new { x = centerX - 0.07f, y = centerY + 0.02f, r = 0.07f },
            new { x = centerX, y = centerY - 0.04f, r = 0.06f }
        };

        foreach ( var c in circles )
            verts.AddRange( geometryGenerator.GenerateCircleVertices(
                c.x, c.y, c.r,
                1f, 1f, 1f,
                0.9f, 0.95f, 1f,
                20 ) );

        clouds.Add( new Mesh2D( Gl, verts.ToArray() ) );
    }

    private static void SetupGrass( GL Gl )
    {
        var grassVerts = geometryGenerator.GenerateGrassVertices( 60 );

        grass = new Mesh2D( Gl, grassVerts );
    }

    private static void SetupFlowers( GL Gl )
    {
        var flowerData = new[]
        {
            new { x = -0.6f, y = -0.7f, type = 0 },
            new { x = -0.2f, y = -0.8f, type = 1 },
            new { x =  0.3f, y = -0.75f, type = 2 },
            new { x =  0.7f, y = -0.85f, type = 3 },
            new { x = -0.85f, y = -0.9f, type = 1 },
            new { x =  0.0f, y = -0.95f, type = 3 }
        };

        var flowerColors = new[]
        {
            new float[] { 1.0f, 0.6f, 0.8f,  1.0f, 1.0f, 0.3f },
            new float[] { 1.0f, 1.0f, 0.3f,  0.8f, 0.5f, 0.1f },
            new float[] { 0.7f, 0.4f, 1.0f,  1.0f, 1.0f, 0.3f },
            new float[] { 1.0f, 0.3f, 0.3f,  1.0f, 1.0f, 0.3f }
        };

        foreach ( var f in flowerData )
        {
            var colors = flowerColors[ f.type ];

            float petalRadius = 0.06f + ( f.type % 2 ) * 0.01f;
            float centerRadius = 0.025f;
            int petalCount = 5 + ( f.type % 2 );

            var flowerVerts = geometryGenerator.GenerateFlowerVertices(
                f.x, f.y,
                petalRadius, centerRadius,
                colors[ 0 ], colors[ 1 ], colors[ 2 ],
                colors[ 3 ], colors[ 4 ], colors[ 5 ],
                petalCount
            );

            flowers.Add( new Mesh2D( Gl, flowerVerts.ToArray() ) );
        }
    }

    private static void SetupButterflies( GL Gl )
    {
        var butterflyPositions = new[]
        {
            new { x = -0.6f, y = -0.7f, scale = 1.0f, type = 0 },
            new { x = -0.2f, y = -0.8f, scale = 0.9f, type = 1 },
            new { x =  0.3f, y = -0.75f, scale = 1.1f, type = 2 },
            new { x =  0.7f, y = -0.85f, scale = 0.95f, type = 0 }
        };

        var butterflyColors = new[]
        {
            new float[] { 1.0f, 0.5f, 0.1f,  0.2f, 0.1f, 0.1f },
            new float[] { 0.3f, 0.6f, 1.0f,  0.1f, 0.1f, 0.2f },
            new float[] { 1.0f, 0.4f, 0.7f,  0.2f, 0.1f, 0.15f }
        };

        foreach ( var b in butterflyPositions )
        {
            var colors = butterflyColors[ b.type ];
            float offsetX = ( b.x > 0 ? 0.03f : -0.03f );

            var butterflyVerts = geometryGenerator.GenerateButterflyVertices(
                b.x + offsetX, b.y + 0.05f, b.scale,
                colors[ 0 ], colors[ 1 ], colors[ 2 ],
                colors[ 3 ], colors[ 4 ], colors[ 5 ]
            );

            butterflies.Add( new Mesh2D( Gl, butterflyVerts.ToArray() ) );
        }
    }

    public void Render( GL Gl )
    {
        meadow.Render( Gl, PrimitiveType.Triangles );

        sun.Render( Gl, PrimitiveType.TriangleFan );

        foreach ( var c in clouds )
            c.Render( Gl, PrimitiveType.TriangleFan );

        grass.Render( Gl, PrimitiveType.Triangles );

        foreach ( var f in flowers )
            f.Render( Gl, PrimitiveType.TriangleFan );

        foreach ( var b in butterflies )
            b.Render( Gl, PrimitiveType.TriangleFan );

        Gl.BindVertexArray( 0 );
    }

    public void Close()
    {
        meadow.Dispose();

        sun.Dispose();

        foreach ( var c in clouds )
            c.Dispose();

        grass.Dispose();

        foreach ( var f in flowers )
            f.Dispose();

        foreach ( var b in butterflies )
            b.Dispose();
    }
}