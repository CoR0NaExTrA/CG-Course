using Assimp;
using Silk.NET.OpenGL;

namespace Battleship3D.Models;

public class Mesh
{
    private GL gl;

    public uint VAO;
    private uint VBO;

    public int VertexCount;

    public Mesh( GL gl, string path )
    {
        this.gl = gl;

        AssimpContext importer = new();
        Scene scene = importer.ImportFile( path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals );

        List<float> vertices = new();

        foreach ( var mesh in scene.Meshes )
        {
            for ( int i = 0; i < mesh.VertexCount; i++ )
            {
                vertices.Add( mesh.Vertices[ i ].X );
                vertices.Add( mesh.Vertices[ i ].Y );
                vertices.Add( mesh.Vertices[ i ].Z );

                vertices.Add( mesh.Normals[ i ].X );
                vertices.Add( mesh.Normals[ i ].Y );
                vertices.Add( mesh.Normals[ i ].Z );

                if ( mesh.TextureCoordinateChannelCount > 0 )
                {
                    vertices.Add( mesh.TextureCoordinateChannels[ 0 ][ i ].X );
                    vertices.Add( mesh.TextureCoordinateChannels[ 0 ][ i ].Y );
                }
                else
                {
                    vertices.Add( 0 );
                    vertices.Add( 0 );
                }
            }
        }

        VertexCount = vertices.Count / 8;

        VAO = gl.GenVertexArray();
        VBO = gl.GenBuffer();

        gl.BindVertexArray( VAO );
        gl.BindBuffer( BufferTargetARB.ArrayBuffer, VBO );

        unsafe
        {
            fixed ( float* v = vertices.ToArray() )
            {
                gl.BufferData(
                    BufferTargetARB.ArrayBuffer,
                    ( nuint )( vertices.Count * sizeof( float ) ),
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
        gl.DrawArrays( Silk.NET.OpenGL.PrimitiveType.Triangles, 0, ( uint )VertexCount );
    }
}
