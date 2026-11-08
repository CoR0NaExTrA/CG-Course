namespace Meadow.Generators;

public class GeometryGenerator
{
    public float[] GenerateCircleVertices( float centerX, float centerY, float radius,
        float innerR, float innerG, float innerB,
        float outerRCol, float outerGCol, float outerBCol, int segments = 32 )
    {
        var vertices = new List<float>();

        vertices.AddRange( [ centerX, centerY, 0.0f, innerR, innerG, innerB ] );

        for ( int i = 0; i <= segments; i++ )
        {
            float angle = i * 2.0f * 3.14159f / segments;
            float x = centerX + radius * MathF.Cos( angle );
            float y = centerY + radius * MathF.Sin( angle );

            vertices.AddRange( [ x, y, 0.0f, outerRCol, outerGCol, outerBCol ] );
        }

        return vertices.ToArray();
    }

    public float[] GenerateGrassVertices( int bladeCount = 80 )
    {
        var vertices = new List<float>();
        var rng = new Random( 42 );

        for ( int i = 0; i < bladeCount; i++ )
        {
            float x = -1.0f + rng.NextSingle() * 2.0f;
            float baseY = -1.0f + rng.NextSingle() * 1.0f;

            float height = 0.05f + rng.NextSingle() * 0.06f;
            float width = 0.01f + rng.NextSingle() * 0.012f;

            float greenVar = 0.35f + rng.NextSingle() * 0.45f;
            float r = 0.08f + rng.NextSingle() * 0.1f;
            float g = greenVar;
            float b = 0.08f + rng.NextSingle() * 0.1f;

            vertices.AddRange( [ x - width, baseY - height, 0.0f, r, g * 0.92f, b ] );
            vertices.AddRange( [ x + width, baseY - height, 0.0f, r, g * 0.92f, b ] );
            vertices.AddRange( [ x, baseY, 0.0f, r, g, b ] );
        }

        return vertices.ToArray();
    }

    public float[] GenerateFlowerVertices( float centerX, float centerY, float petalRadius, float centerRadius,
        float petalR, float petalG, float petalB,
        float centerR, float centerG, float centerB,
        int petalCount = 5 )
    {
        var vertices = new List<float>();

        for ( int i = 0; i < petalCount; i++ )
        {
            float angle = i * 2.0f * 3.14159f / petalCount;
            float px = centerX + petalRadius * 0.9f * MathF.Cos( angle );
            float py = centerY + petalRadius * 0.9f * MathF.Sin( angle );

            var petalVerts = GenerateCircleVertices( px, py, petalRadius * 0.45f,
                petalR, petalG, petalB,
                petalR * 0.9f, petalG * 0.9f, petalB * 0.9f,
                segments: 16 );
            vertices.AddRange( petalVerts );
        }

        var centerVerts = GenerateCircleVertices( centerX, centerY, centerRadius,
            centerR, centerG, centerB,
            centerR * 0.85f, centerG * 0.85f, centerB * 0.85f,
            segments: 20 );
        vertices.AddRange( centerVerts );

        return vertices.ToArray();
    }

    public float[] GenerateButterflyVertices( float centerX, float centerY, float scale,
        float wingR, float wingG, float wingB,
        float bodyR, float bodyG, float bodyB )
    {
        var vertices = new List<float>();

        var wings = new[]
        {
            new { x = centerX - scale * 0.04f, y = centerY + scale * 0.03f, r = scale * 0.035f },
            new { x = centerX + scale * 0.04f, y = centerY + scale * 0.03f, r = scale * 0.035f },
            new { x = centerX - scale * 0.03f, y = centerY - scale * 0.02f, r = scale * 0.025f },
            new { x = centerX + scale * 0.03f, y = centerY - scale * 0.02f, r = scale * 0.025f }
        };

        foreach ( var w in wings )
        {
            var wingVerts = GenerateCircleVertices( w.x, w.y, w.r,
                wingR, wingG, wingB,
                wingR * 0.85f, wingG * 0.85f, wingB * 0.85f,
                segments: 14 );
            vertices.AddRange( wingVerts );
        }

        var body1 = GenerateCircleVertices( centerX, centerY + scale * 0.01f, scale * 0.012f,
            bodyR, bodyG, bodyB, bodyR * 0.7f, bodyG * 0.7f, bodyB * 0.7f, 12 );
        var body2 = GenerateCircleVertices( centerX, centerY - scale * 0.01f, scale * 0.01f,
            bodyR, bodyG, bodyB, bodyR * 0.7f, bodyG * 0.7f, bodyB * 0.7f, 12 );
        vertices.AddRange( body1 );
        vertices.AddRange( body2 );

        return vertices.ToArray();
    }
}
