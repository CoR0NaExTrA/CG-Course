using Silk.NET.OpenGL;
using System.Numerics;

namespace MobiusStrip;

class MobiusMesh
{
    private readonly GL _gl;

    public uint VAO { get; private set; }
    public uint VBO { get; private set; }
    public uint EBO { get; private set; }

    public float[] Vertices { get; private set; }
    public uint[] Indices { get; private set; }
    public int IndexCount { get; private set; }

    public MobiusMesh( GL gl ) => _gl = gl;

    public void Generate( int segU, int segV )
    {
        var vertices = new List<float>();
        var indices = new List<uint>();
        float width = 0.3f;

        // Генерация вершин
        for ( int i = 0; i <= segU; i++ )
        {
            float u = ( float )i / segU * MathF.PI * 2;

            for ( int j = 0; j <= segV; j++ )
            {
                float vv = ( ( float )j / segV * 2 - 1 ) * width;

                // Позиция
                float x = ( 1 + vv * MathF.Cos( u / 2 ) ) * MathF.Cos( u );
                float y = ( 1 + vv * MathF.Cos( u / 2 ) ) * MathF.Sin( u );
                float z = vv * MathF.Sin( u / 2 );

                // Нормаль через частные производные
                Vector3 pos = new( x, y, z );
                Vector3 du = ComputeDu( u, vv );
                Vector3 dv = ComputeDv( u );
                Vector3 normal = Vector3.Normalize( Vector3.Cross( du, dv ) );

                vertices.AddRange( new[] { pos.X, pos.Y, pos.Z, normal.X, normal.Y, normal.Z } );
            }
        }

        // Замыкание нормалей
        int stride = 6;
        for ( int j = 0; j <= segV; j++ )
        {
            int first = j * stride;
            int last = ( segU * ( segV + 1 ) + j ) * stride;
            
            vertices[ last + 3 ] = - vertices[ first + 3 ];
            vertices[ last + 4 ] = - vertices[ first + 4 ];
            vertices[ last + 5 ] = - vertices[ first + 5 ];
        }

        // Индексы
        for ( uint i = 0; i < segU; i++ )
        {
            for ( uint j = 0; j < segV; j++ )
            {
                uint a = i * ( uint )( segV + 1 ) + j;
                uint b = a + ( uint )( segV + 1 );

                indices.AddRange( new[] { a, b, a + 1, b, b + 1, a + 1 } );
            }
        }

        Vertices = vertices.ToArray();
        Indices = indices.ToArray();
        IndexCount = Indices.Length;
    }

    private Vector3 ComputeDu( float u, float vv ) => new(
        -MathF.Sin( u ) * ( 1 + vv * MathF.Cos( u / 2 ) ) - vv * 0.5f * MathF.Sin( u / 2 ) * MathF.Cos( u ),
         MathF.Cos( u ) * ( 1 + vv * MathF.Cos( u / 2 ) ) - vv * 0.5f * MathF.Sin( u / 2 ) * MathF.Sin( u ),
         vv * 0.5f * MathF.Cos( u / 2 )
    );

    private Vector3 ComputeDv( float u ) => new(
        MathF.Cos( u / 2 ) * MathF.Cos( u ),
        MathF.Cos( u / 2 ) * MathF.Sin( u ),
        MathF.Sin( u / 2 )
    );

    public unsafe void UploadToGPU()
    {
        VAO = _gl.GenVertexArray();
        VBO = _gl.GenBuffer();
        EBO = _gl.GenBuffer();

        _gl.BindVertexArray( VAO );

        // Вершины
        _gl.BindBuffer( GLEnum.ArrayBuffer, VBO );
        fixed ( float* v = Vertices )
            _gl.BufferData( GLEnum.ArrayBuffer,
                ( nuint )( Vertices.Length * sizeof( float ) ),
                v, GLEnum.StaticDraw );

        // Индексы
        _gl.BindBuffer( GLEnum.ElementArrayBuffer, EBO );
        fixed ( uint* i = Indices )
            _gl.BufferData( GLEnum.ElementArrayBuffer,
                ( nuint )( Indices.Length * sizeof( uint ) ),
                i, GLEnum.StaticDraw );

        // Атрибуты
        int stride = 6 * sizeof( float );

        _gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, ( uint )stride, ( void* )0 );
        _gl.EnableVertexAttribArray( 0 );

        _gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, ( uint )stride, ( void* )( 3 * sizeof( float ) ) );
        _gl.EnableVertexAttribArray( 1 );
    }

    public unsafe void Draw()
    {
        _gl.BindVertexArray( VAO );
        _gl.DrawElements( GLEnum.Triangles, ( uint )IndexCount, GLEnum.UnsignedInt, null );
    }
}