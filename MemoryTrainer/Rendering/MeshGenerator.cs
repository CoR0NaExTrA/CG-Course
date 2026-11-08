using Silk.NET.OpenGL;

namespace MemoryTrainer.Rendering;

public readonly struct MeshData
{
    public uint VAO { get; }
    public uint VBO { get; }
    public int VertexCount { get; }

    public MeshData( uint vao, uint vbo, int vertexCount )
    {
        VAO = vao;
        VBO = vbo;
        VertexCount = vertexCount;
    }

    public void Bind( GL gl ) => gl.BindVertexArray( VAO );
}

public static class MeshGenerator
{
    public static MeshData CreateCube( GL gl, float size, float height )
    {
        var vertices = GenerateCubeVertices( size, height );
        return CreateMesh( gl, vertices );
    }

    private static float[] GenerateCubeVertices( float s, float h )
    {
        return
        [
            0,h,0, 0,1,0, 0,0,   s,h,0, 0,1,0, 1,0,   s,h,s, 0,1,0, 1,1,
            0,h,0, 0,1,0, 0,0,   s,h,s, 0,1,0, 1,1,   0,h,s, 0,1,0, 0,1,

            0,0,0, 0,-1,0, 0,0,   s,0,s, 0,-1,0, 1,1,   s,0,0, 0,-1,0, 1,0,
            0,0,0, 0,-1,0, 0,0,   0,0,s, 0,-1,0, 0,1,   s,0,s, 0,-1,0, 1,1,
            
            0,0,0, -1,0,0, 0,0,   0,h,s, -1,0,0, 1,1,   0,h,0, -1,0,0, 1,0,
            0,0,0, -1,0,0, 0,0,   0,0,s, -1,0,0, 0,1,   0,h,s, -1,0,0, 1,1,
            
            s,0,0, 1,0,0, 0,0,   s,h,0, 1,0,0, 1,0,   s,h,s, 1,0,0, 1,1,
            s,0,0, 1,0,0, 0,0,   s,h,s, 1,0,0, 1,1,   s,0,s, 1,0,0, 0,1,
            
            0,0,s, 0,0,1, 0,0,   s,h,s, 0,0,1, 1,1,   s,0,s, 0,0,1, 1,0,
            0,0,s, 0,0,1, 0,0,   0,h,s, 0,0,1, 0,1,   s,h,s, 0,0,1, 1,1,
            
            0,0,0, 0,0,-1, 0,0,   s,0,0, 0,0,-1, 1,0,   s,h,0, 0,0,-1, 1,1,
            0,0,0, 0,0,-1, 0,0,   s,h,0, 0,0,-1, 1,1,   0,h,0, 0,0,-1, 0,1,
        ];
    }

    private static MeshData CreateMesh( GL gl, float[] vertices )
    {
        uint vao = gl.GenVertexArray();
        uint vbo = gl.GenBuffer();

        gl.BindVertexArray( vao );
        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );

        unsafe
        {
            fixed ( float* ptr = vertices )
            {
                gl.BufferData( GLEnum.ArrayBuffer,
                    ( nuint )( vertices.Length * sizeof( float ) ),
                    ptr, GLEnum.StaticDraw );
            }
        }

        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, 8 * sizeof( float ), 0 );
        gl.EnableVertexAttribArray( 0 );

        gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, 8 * sizeof( float ), 3 * sizeof( float ) );
        gl.EnableVertexAttribArray( 1 );

        gl.VertexAttribPointer( 2, 2, GLEnum.Float, false, 8 * sizeof( float ), 6 * sizeof( float ) );
        gl.EnableVertexAttribArray( 2 );

        return new MeshData( vao, vbo, vertices.Length / 8 );
    }
}