using Silk.NET.OpenGL;

namespace Meadow.Graphics;

public unsafe class Mesh2D : IDisposable
{
    private GL gl;

    public uint Vao;
    public uint Vbo;
    public int VertexCount;

    public Mesh2D( GL gl, float[] vertices )
    {
        this.gl = gl;

        VertexCount = vertices.Length / 6;

        Vao = gl.GenVertexArray();
        gl.BindVertexArray( Vao );

        Vbo = gl.GenBuffer();
        gl.BindBuffer( BufferTargetARB.ArrayBuffer, Vbo );

        fixed ( void* v = &vertices[ 0 ] )
        {
            gl.BufferData(
                BufferTargetARB.ArrayBuffer,
                ( nuint )( vertices.Length * sizeof( float ) ),
                v,
                BufferUsageARB.StaticDraw );
        }

        gl.VertexAttribPointer( 0, 3, VertexAttribPointerType.Float, false,
            6 * sizeof( float ), null );
        gl.EnableVertexAttribArray( 0 );

        gl.VertexAttribPointer( 1, 3, VertexAttribPointerType.Float, false,
            6 * sizeof( float ), ( void* )( 3 * sizeof( float ) ) );
        gl.EnableVertexAttribArray( 1 );

        gl.BindBuffer( BufferTargetARB.ArrayBuffer, 0 );
        gl.BindVertexArray( 0 );
    }

    public void Render( GL gl, PrimitiveType type )
    {
        gl.BindVertexArray( Vao );
        gl.DrawArrays( type, 0, ( uint )VertexCount );
    }

    public void Dispose()
    {
        gl.DeleteBuffer( Vbo );
        gl.DeleteVertexArray( Vao );
    }
}