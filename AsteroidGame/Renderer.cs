using Silk.NET.OpenGL;
using System.Numerics;

namespace AsteroidGame;

public unsafe class Renderer
{
    GL gl;
    uint vao;
    uint vbo;
    GameShader shader;
    int projectionLocation;

    public Renderer( GL gl )
    {
        string vertexShader = @"
            #version 330 core
            layout (location = 0) in vec2 aPos;

            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * vec4(aPos, 0.0, 1.0);
            }
            ";

        string fragmentShader = @"
        #version 330 core
        out vec4 FragColor;

        void main()
        {
            FragColor = vec4(1.0,1.0,1.0,1.0);
        }
        ";

        this.gl = gl;

        shader = new GameShader( gl, vertexShader, fragmentShader );
        projectionLocation = gl.GetUniformLocation( shader.Handle, "projection" );

        vao = gl.GenVertexArray();
        vbo = gl.GenBuffer();

        gl.BindVertexArray( vao );

        gl.BindBuffer( BufferTargetARB.ArrayBuffer, vbo );

        gl.VertexAttribPointer(
            0,
            2,
            VertexAttribPointerType.Float,
            false,
            2 * sizeof( float ),
            ( void* )0 );

        gl.EnableVertexAttribArray( 0 );
    }

    public void SetProjection( int width, int height )
    {
        Matrix4x4 projection = Matrix4x4.CreateOrthographicOffCenter(
            0,
            width,
            height,
            0,
            -1,
            1 );

        shader.Use();

        unsafe
        {
            gl.UniformMatrix4(
                projectionLocation,
                1,
                false,
                ( float* )&projection );
        }
    }

    public void Draw( float[] vertices, PrimitiveType type )
    {
        shader.Use();

        gl.BindVertexArray( vao );
        gl.BindBuffer( BufferTargetARB.ArrayBuffer, vbo );

        fixed ( float* v = vertices )
        {
            gl.BufferData(
                BufferTargetARB.ArrayBuffer,
                ( nuint )( vertices.Length * sizeof( float ) ),
                v,
                BufferUsageARB.DynamicDraw );
        }

        gl.DrawArrays( type, 0, ( uint )( vertices.Length / 2 ) );
    }
}
