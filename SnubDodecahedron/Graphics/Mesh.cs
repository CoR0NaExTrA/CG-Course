using Silk.NET.OpenGL;
using SnubDodecahedron.Models;
namespace SnubDodecahedron.Graphics;

public class Mesh
{
    private GL gl;
    private uint vao, vbo, ebo;
    private int indexCount;

    private uint edgeEbo;
    private int edgeIndexCount;

    public unsafe Mesh( GL gl, Vertex[] vertices, uint[] indices )
    {
        this.gl = gl;
        indexCount = indices.Length;

        var edgeIndices = Geometry.SnubDodecahedron.CreateEdgeIndices();

        edgeIndexCount = edgeIndices.Length;

        edgeEbo = gl.GenBuffer();
        gl.BindBuffer( GLEnum.ElementArrayBuffer, edgeEbo );

        fixed ( uint* e = &edgeIndices[ 0 ] )
        {
            gl.BufferData( GLEnum.ElementArrayBuffer,
                ( nuint )( edgeIndices.Length * sizeof( uint ) ),
                e,
                GLEnum.StaticDraw );
        }

        vao = gl.GenVertexArray();
        vbo = gl.GenBuffer();
        ebo = gl.GenBuffer();

        gl.BindVertexArray( vao );

        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );
        fixed ( Vertex* v = &vertices[ 0 ] )
        {
            gl.BufferData( GLEnum.ArrayBuffer,
                ( nuint )( vertices.Length * sizeof( Vertex ) ),
                v,
                GLEnum.StaticDraw );
        }

        gl.BindBuffer( GLEnum.ElementArrayBuffer, ebo );
        fixed ( uint* i = &indices[ 0 ] )
        {
            gl.BufferData( GLEnum.ElementArrayBuffer,
                ( nuint )( indices.Length * sizeof( uint ) ),
                i,
                GLEnum.StaticDraw );
        }

        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )0 );
        gl.EnableVertexAttribArray( 0 );

        gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )12 );
        gl.EnableVertexAttribArray( 1 );

        gl.VertexAttribPointer( 2, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )24 );
        gl.EnableVertexAttribArray( 2 );
    }

    public unsafe void Draw( DodecahedronShader shader )
    {
        int isWireframeLoc = gl.GetUniformLocation( shader.GetProgram(), "isWireframe" );

        gl.BindVertexArray( vao );

        // 🔷 Рисуем грани
        gl.BindBuffer( GLEnum.ElementArrayBuffer, ebo );
        gl.Uniform1( isWireframeLoc, 0 );
        gl.DrawElements( GLEnum.Triangles, ( uint )indexCount, GLEnum.UnsignedInt, null );

        // 🔷 Рисуем РЁБРА (без диагоналей)
        gl.BindBuffer( GLEnum.ElementArrayBuffer, edgeEbo );
        gl.LineWidth( 3f );
        gl.Uniform1( isWireframeLoc, 1 );
        gl.DrawElements( GLEnum.Lines, ( uint )edgeIndexCount, GLEnum.UnsignedInt, null );
    }
}