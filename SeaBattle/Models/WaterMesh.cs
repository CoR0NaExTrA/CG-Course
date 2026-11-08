using Silk.NET.OpenGL;

namespace Battleship3D.Models;

public class WaterMesh
{
    private GL gl;

    public uint VAO;
    private uint VBO;

    public WaterMesh( GL gl )
    {
        this.gl = gl;

        float[] vertices =
        {
            -200f, 0f, -200f,   0,1,0,  0,0,
             200f, 0f, -200f,   0,1,0,  1,0,
             200f, 0f,  200f,   0,1,0,  1,1,

            -200f, 0f, -200f,   0,1,0,  0,0,
             200f, 0f,  200f,   0,1,0,  1,1,
            -200f, 0f,  200f,   0,1,0,  0,1,
        };

        VAO = gl.GenVertexArray();
        VBO = gl.GenBuffer();

        gl.BindVertexArray( VAO );

        gl.BindBuffer( BufferTargetARB.ArrayBuffer, VBO );

        unsafe
        {
            fixed ( float* v = vertices )
            {
                gl.BufferData(
                    BufferTargetARB.ArrayBuffer,
                    ( nuint )( vertices.Length * sizeof( float ) ),
                    v,
                    BufferUsageARB.StaticDraw );
            }


            gl.VertexAttribPointer( 0, 3, VertexAttribPointerType.Float, false, 8 * sizeof( float ), 0 );
            gl.EnableVertexAttribArray( 0 );

            gl.VertexAttribPointer( 1, 3, VertexAttribPointerType.Float, false, 8 * sizeof( float ), ( void* )( 3 * sizeof( float ) ) );
            gl.EnableVertexAttribArray( 1 );

            gl.VertexAttribPointer( 2, 2, VertexAttribPointerType.Float, false, 8 * sizeof( float ), ( void* )( 6 * sizeof( float ) ) );
            gl.EnableVertexAttribArray( 2 );
        }
    }

    public void Draw()
    {
        gl.BindVertexArray( VAO );
        gl.DrawArrays( PrimitiveType.Triangles, 0, 6 );
    }
}